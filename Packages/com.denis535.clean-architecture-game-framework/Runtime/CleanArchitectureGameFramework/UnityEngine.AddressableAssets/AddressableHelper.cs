#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    internal static class AddressableHelper {

        // IsState
        public static bool IsNone<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.None;
        }
        public static bool IsSucceeded<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.Succeeded;
        }
        public static bool IsFailed<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.Failed;
        }

        // LoadAssetAsync
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object {
            return Addressables.LoadAssetAsync<T>( key );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadAssetListAsync<T>(string[] keys) where T : UnityEngine.Object {
            return Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).ChainOperation( assets => {
                return Addressables.ResourceManager.CreateCompletedOperation( (IReadOnlyList<T>) assets.Result, null );
            } );
        }

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefab => {
                return RequireComponentOperation<T>( prefab.Result );
            } );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            return Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).ChainOperation( prefabs => {
                return RequireComponentsOperation<T>( (IReadOnlyList<GameObject>) prefabs.Result );
            } );
        }

        // InstantiateAsync
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefab => {
                return InstantiateOperation<T>( prefab.Result );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefab => {
                return InstantiateOperation<T>( prefab.Result, position, rotation );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Transform? parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefab => {
                return InstantiateOperation<T>( prefab.Result, parent );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation, Transform? parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefab => {
                return InstantiateOperation<T>( prefab.Result, position, rotation, parent );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Func<GameObject, T> instanceProvider) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefab => {
                return InstantiateOperation<T>( prefab.Result, instanceProvider );
            } );
        }

        // Wait
        public static void Wait<T>(this AsyncOperationHandle<T> handle) {
            if (!handle.IsFailed()) {
                handle.WaitForCompletion();
                if (handle.IsSucceeded()) {
                    return;
                }
            }
            throw handle.OperationException;
        }
        public static async ValueTask WaitAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (!handle.IsFailed()) {
                await handle.Task.WaitAsync( cancellationToken );
                cancellationToken.ThrowIfCancellationRequested();
                if (handle.IsSucceeded()) {
                    return;
                }
            }
            throw handle.OperationException;
        }

        // GetResult
        public static T GetResult<T>(this AsyncOperationHandle<T> handle) {
            if (!handle.IsFailed()) {
                var result = handle.WaitForCompletion();
                if (handle.IsSucceeded()) {
                    return result;
                }
            }
            throw handle.OperationException;
        }
        public static async ValueTask<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (!handle.IsFailed()) {
                var result = await handle.Task.WaitAsync( cancellationToken );
                cancellationToken.ThrowIfCancellationRequested();
                if (handle.IsSucceeded()) {
                    return result;
                }
            }
            throw handle.OperationException;
        }

        // Helpers
        private static AsyncOperationHandle<T> ChainOperation<TDependency, T>(this AsyncOperationHandle<TDependency> dependency, Func<AsyncOperationHandle<TDependency>, AsyncOperationHandle<T>> operationProvider) {
            return Addressables.ResourceManager.CreateChainOperation( dependency, dependency => {
                if (dependency.IsSucceeded()) {
                    var operation = operationProvider( dependency );
                    operation.Destroyed += i => {
                        Addressables.Release( dependency );
                    };
                    return operation;
                } else {
                    var operation = Addressables.ResourceManager.CreateCompletedOperationWithException<T>( default!, dependency.OperationException );
                    operation.Destroyed += i => {
                        Addressables.Release( dependency );
                    };
                    return operation;
                }
            } );
        }
        // Helpers
        private static AsyncOperationHandle<T> RequireComponentOperation<T>(GameObject gameObject) where T : Component {
            try {
                return Addressables.ResourceManager.CreateCompletedOperation( gameObject.RequireComponent<T>(), null );
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<IReadOnlyList<T>> RequireComponentsOperation<T>(IReadOnlyList<GameObject> gameObjects) where T : Component {
            try {
                return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( gameObjects.Select( i => i.RequireComponent<T>() ).ToList(), null );
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<IReadOnlyList<T>>( null!, ex );
            }
        }
        // Helpers
        private static AsyncOperationHandle<T> InstantiateOperation<T>(GameObject prefab) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>() );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => {
                    UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                };
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> InstantiateOperation<T>(GameObject prefab, Transform? parent) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>(), parent );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => {
                    UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                };
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> InstantiateOperation<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>(), position, rotation );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => {
                    UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                };
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> InstantiateOperation<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform? parent) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>(), position, rotation, parent );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => {
                    UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                };
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> InstantiateOperation<T>(GameObject prefab, Func<GameObject, T> instanceProvider) where T : Component {
            try {
                var instance = instanceProvider( prefab );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => {
                    UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                };
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }

    }
}

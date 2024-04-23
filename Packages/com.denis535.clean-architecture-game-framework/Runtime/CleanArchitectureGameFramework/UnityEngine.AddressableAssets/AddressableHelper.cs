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
            return Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).Chain( i => {
                if (i.IsSucceeded()) {
                    var assets = i.Result;
                    return Addressables.ResourceManager.CreateCompletedOperation( (IReadOnlyList<T>) assets, null );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<IReadOnlyList<T>>( null!, i.OperationException );
            } );
        }

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    return CreateCastOperation<T>( prefab );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            return Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefabs = i.Result;
                    return CreateCastOperation<T>( (IReadOnlyList<GameObject>) prefabs );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<IReadOnlyList<T>>( null!, i.OperationException );
            } );
        }

        // InstantiateAsync
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    return CreateInstantiateOperation<T>( prefab );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    return CreateInstantiateOperation<T>( prefab, position, rotation );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Transform? parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    return CreateInstantiateOperation<T>( prefab, parent );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation, Transform? parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    return CreateInstantiateOperation<T>( prefab, position, rotation, parent );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Func<GameObject, T> instanceProvider) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    return CreateInstantiateOperation<T>( prefab, instanceProvider );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }

        // Wait
        public static void Wait<T>(this AsyncOperationHandle<T> handle) {
            if (handle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                handle.WaitForCompletion();
                if (handle.Status is AsyncOperationStatus.Succeeded) {
                    return;
                }
            }
            throw handle.OperationException;
        }
        public static async ValueTask WaitAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (handle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                await handle.Task.WaitAsync( cancellationToken );
                cancellationToken.ThrowIfCancellationRequested();
                if (handle.Status is AsyncOperationStatus.Succeeded) {
                    return;
                }
            }
            throw handle.OperationException;
        }

        // GetResult
        public static T GetResult<T>(this AsyncOperationHandle<T> handle) {
            if (handle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = handle.WaitForCompletion();
                if (handle.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw handle.OperationException;
        }
        public static async ValueTask<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (handle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await handle.Task.WaitAsync( cancellationToken );
                cancellationToken.ThrowIfCancellationRequested();
                if (handle.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw handle.OperationException;
        }

        // Helpers
        private static AsyncOperationHandle<T2> Chain<T1, T2>(this AsyncOperationHandle<T1> handle1, Func<AsyncOperationHandle<T1>, AsyncOperationHandle<T2>> handle2Provider) {
            var handle2 = Addressables.ResourceManager.CreateChainOperation( handle1, handle2Provider );
            handle2.Destroyed += i => Addressables.Release( handle1 );
            return handle2;
        }
        // Helpers
        private static AsyncOperationHandle<T> CreateCastOperation<T>(GameObject gameObject) where T : Component {
            try {
                return Addressables.ResourceManager.CreateCompletedOperation( gameObject.RequireComponent<T>(), null );
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<IReadOnlyList<T>> CreateCastOperation<T>(IReadOnlyList<GameObject> gameObjects) where T : Component {
            try {
                return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( gameObjects.Select( i => i.RequireComponent<T>() ).ToList(), null );
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<IReadOnlyList<T>>( null!, ex );
            }
        }
        // Helpers
        private static AsyncOperationHandle<T> CreateInstantiateOperation<T>(GameObject prefab) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>() );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> CreateInstantiateOperation<T>(GameObject prefab, Transform? parent) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>(), parent );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> CreateInstantiateOperation<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>(), position, rotation );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> CreateInstantiateOperation<T>(GameObject prefab, Vector3 position, Quaternion rotation, Transform? parent) where T : Component {
            try {
                var instance = UnityEngine.Object.Instantiate( prefab.RequireComponent<T>(), position, rotation, parent );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }
        private static AsyncOperationHandle<T> CreateInstantiateOperation<T>(GameObject prefab, Func<GameObject, T> instanceProvider) where T : Component {
            try {
                var instance = instanceProvider( prefab );
                var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                operation.Destroyed += i => UnityEngine.Object.Destroy( ((T) i.Result).gameObject );
                return operation;
            } catch (Exception ex) {
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, ex );
            }
        }

    }
}

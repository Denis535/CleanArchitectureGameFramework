#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    internal static class AddressableHelper {

        // LoadAssetAsync
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object {
            return Addressables.LoadAssetAsync<T>( key );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadAssetListAsync<T>(string[] keys) where T : UnityEngine.Object {
            return Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).ChainOperation( assetsHandle => {
                return Addressables.ResourceManager.CreateCompletedOperation( (IReadOnlyList<T>) assetsHandle.Result, null );
            } );
        }

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefabHandle => {
                return RequireComponentOperation<T>( prefabHandle.Result );
            } );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            return Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).ChainOperation( prefabsHandle => {
                return RequireComponentsOperation<T>( (IReadOnlyList<GameObject>) prefabsHandle.Result );
            } );
        }

        // InstantiateAsync
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefabsHandle => {
                return InstantiateOperation<T>( prefabsHandle.Result );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefabsHandle => {
                return InstantiateOperation<T>( prefabsHandle.Result, position, rotation );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Transform? parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefabsHandle => {
                return InstantiateOperation<T>( prefabsHandle.Result, parent );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation, Transform? parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefabsHandle => {
                return InstantiateOperation<T>( prefabsHandle.Result, position, rotation, parent );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Func<GameObject, T> instanceProvider) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).ChainOperation( prefabsHandle => {
                return InstantiateOperation<T>( prefabsHandle.Result, instanceProvider );
            } );
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

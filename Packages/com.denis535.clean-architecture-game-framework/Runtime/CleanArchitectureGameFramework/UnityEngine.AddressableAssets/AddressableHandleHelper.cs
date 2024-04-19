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

    internal static class AddressableHandleHelper {

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
                    var assets = (IReadOnlyList<T>) i.Result;
                    return Addressables.ResourceManager.CreateCompletedOperation( assets, null );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<IReadOnlyList<T>>( null!, i.OperationException );
            } );
        }

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    var component = (T?) prefab.GetComponent<T>();
                    if (component != null) {
                        return Addressables.ResourceManager.CreateCompletedOperation<T>( component, null );
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            return Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefabs = i.Result;
                    var components = prefabs.Select( i => i.GetComponent<T>() ).ToList();
                    if (components.All( i => i != null )) {
                        return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( components, null );
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<IReadOnlyList<T>>( null!, i.OperationException );
            } );
        }

        // InstantiateAsync
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    var component = prefab.GetComponent<T>();
                    if (component != null) {
                        var instance = GameObject.Instantiate( component );
                        var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                        operation.Destroyed += i => GameObject.Destroy( ((T) i.Result).gameObject );
                        return operation;
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    var component = prefab.GetComponent<T>();
                    if (component != null) {
                        var instance = GameObject.Instantiate( component, position, rotation );
                        var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                        operation.Destroyed += i => GameObject.Destroy( ((T) i.Result).gameObject );
                        return operation;
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Transform parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    var component = prefab.GetComponent<T>();
                    if (component != null) {
                        var instance = GameObject.Instantiate( component, parent );
                        var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                        operation.Destroyed += i => GameObject.Destroy( ((T) i.Result).gameObject );
                        return operation;
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation, Transform parent) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    var component = prefab.GetComponent<T>();
                    if (component != null) {
                        var instance = GameObject.Instantiate( component, position, rotation, parent );
                        var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                        operation.Destroyed += i => GameObject.Destroy( ((T) i.Result).gameObject );
                        return operation;
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }
        public static AsyncOperationHandle<T> InstantiateAsync<T>(string key, Func<T, T> instanceProvider) where T : notnull, Component {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( i => {
                if (i.IsSucceeded()) {
                    var prefab = i.Result;
                    var component = prefab.GetComponent<T>();
                    if (component != null) {
                        var instance = instanceProvider( component );
                        var operation = Addressables.ResourceManager.CreateCompletedOperation( instance, null );
                        operation.Destroyed += i => GameObject.Destroy( ((T) i.Result).gameObject );
                        return operation;
                    }
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
                return Addressables.ResourceManager.CreateCompletedOperationWithException<T>( null!, i.OperationException );
            } );
        }

        // GetResultAsync
        public static async ValueTask<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (handle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await handle.Task.WaitAsync( cancellationToken );
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

    }
}

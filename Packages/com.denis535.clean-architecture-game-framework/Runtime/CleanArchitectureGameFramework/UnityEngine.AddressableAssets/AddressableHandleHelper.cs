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

        // LoadAssetAsync
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object {
            var handle = Addressables.LoadAssetAsync<T>( key );
            return handle;
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadAssetListAsync<T>(string[] keys) where T : UnityEngine.Object {
            var handle = Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union );
            return Addressables.ResourceManager.CreateChainOperation<IReadOnlyList<T>, IList<T>>( handle, i => Addressables.ResourceManager.CreateCompletedOperation( (IReadOnlyList<T>) handle.Result, null ) );
        }

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : Component {
            var handle = Addressables.LoadAssetAsync<GameObject>( key );
            return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( handle, prefab => {
                var component = (T?) prefab.Result.GetComponent<T>();
                if (component != null) {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( component, null );
                } else {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
            } );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            var handle = Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union );
            return Addressables.ResourceManager.CreateChainOperation<IReadOnlyList<T>, IList<GameObject>>( handle, prefabs => {
                var components = new List<T>( prefabs.Result.Count );
                foreach (var prefab in prefabs.Result) {
                    var component = (T?) prefab.GetComponent<T>();
                    if (component != null) {
                        components.Add( component );
                    } else {
                        return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( null!, $"Component '{typeof( T )}' was not found" );
                    }
                }
                return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( components, null );
            } );
        }

        //// LoadPrefabAsync
        //public static AsyncOperationHandle<T> LoadPrefabAsync<T>(IResourceLocation location) where T : Component {
        //    var handle = Addressables.LoadAssetAsync<GameObject>( location );
        //    return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( handle, prefab => {
        //        var component = (T?) prefab.Result.GetComponent<T>();
        //        if (component != null) {
        //            return Addressables.ResourceManager.CreateCompletedOperation<T>( component, null );
        //        } else {
        //            return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
        //        }
        //    } );
        //}
        //public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(IResourceLocation location) where T : Component {
        //    var handle = Addressables.LoadAssetsAsync<GameObject>( location, null! );
        //    return Addressables.ResourceManager.CreateChainOperation<IReadOnlyList<T>, IList<GameObject>>( handle, prefabs => {
        //        var components = new List<T>( prefabs.Result.Count );
        //        foreach (var prefab in prefabs.Result) {
        //            var component = (T?) prefab.GetComponent<T>();
        //            if (component != null) {
        //                components.Add( component );
        //            } else {
        //                return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( null!, $"Component '{typeof( T )}' was not found" );
        //            }
        //        }
        //        return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( components, null );
        //    } );
        //}

        // GetResultAsync
        public static async Task<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (handle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await handle.Task.WaitAsync( cancellationToken );
                if (handle.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw handle.OperationException;
        }

        // Assert
        public static void Assert_IsValid(this SceneHandleBase handle) {
            Assert.Operation.Message( $"SceneHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded(this SceneHandleBase handle) {
            Assert.Operation.Message( $"SceneHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid(this SceneHandleBase handle) {
            Assert.Operation.Message( $"SceneHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

    }
}

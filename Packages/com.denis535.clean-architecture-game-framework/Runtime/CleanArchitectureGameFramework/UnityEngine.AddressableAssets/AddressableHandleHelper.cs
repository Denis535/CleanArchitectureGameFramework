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

        // Assert
        public static void Assert_IsValid<T>(this AssetHandleBase<T> handle) where T : notnull, UnityEngine.Object {
            Assert.Operation.Message( $"AssetHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this AssetHandleBase<T> handle) where T : notnull, UnityEngine.Object {
            Assert.Operation.Message( $"AssetHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this AssetHandleBase<T> handle) where T : notnull, UnityEngine.Object {
            Assert.Operation.Message( $"AssetHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

        // Assert
        public static void Assert_IsValid<T>(this AssetListHandleBase<T> handle) where T : notnull, UnityEngine.Object {
            Assert.Operation.Message( $"AssetListHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this AssetListHandleBase<T> handle) where T : notnull, UnityEngine.Object {
            Assert.Operation.Message( $"AssetListHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this AssetListHandleBase<T> handle) where T : notnull, UnityEngine.Object {
            Assert.Operation.Message( $"AssetListHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

        // Assert
        public static void Assert_IsValid<T>(this PrefabHandleBase<T> handle) where T : notnull, Component {
            Assert.Operation.Message( $"PrefabHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this PrefabHandleBase<T> handle) where T : notnull, Component {
            Assert.Operation.Message( $"PrefabHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this PrefabHandleBase<T> handle) where T : notnull, Component {
            Assert.Operation.Message( $"PrefabHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

        // Assert
        public static void Assert_IsValid<T>(this PrefabListHandleBase<T> handle) where T : notnull, Component {
            Assert.Operation.Message( $"PrefabListHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this PrefabListHandleBase<T> handle) where T : notnull, Component {
            Assert.Operation.Message( $"PrefabListHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this PrefabListHandleBase<T> handle) where T : notnull, Component {
            Assert.Operation.Message( $"PrefabListHandle {handle} is already valid" ).Valid( !handle.IsValid );
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

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : Component {
            var handle = Addressables.LoadAssetAsync<GameObject>( key );
            return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( handle, i => {
                var result = (T?) i.Result.GetComponent<T>();
                if (result != null) {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( result, null );
                } else {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
            } );
        }
        //public static AsyncOperationHandle<T> LoadPrefabAsync<T>(IResourceLocation location) where T : Component {
        //    var handle = Addressables.LoadAssetAsync<GameObject>( location );
        //    return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( handle, i => {
        //        var result = (T?) i.Result.GetComponent<T>();
        //        if (result != null) {
        //            return Addressables.ResourceManager.CreateCompletedOperation<T>( result, null );
        //        } else {
        //            return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
        //        }
        //    } );
        //}

        // LoadPrefabListAsync
        public static AsyncOperationHandle<IList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            var handle = Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union );
            return Addressables.ResourceManager.CreateChainOperation<IList<T>, IList<GameObject>>( handle, i => {
                var result = new List<T>( i.Result.Count );
                foreach (var item in i.Result) {
                    var item_ = (T?) item.GetComponent<T>();
                    if (item_ != null) {
                        result.Add( item_ );
                    } else {
                        return Addressables.ResourceManager.CreateCompletedOperation<IList<T>>( null!, $"Component '{typeof( T )}' was not found" );
                    }
                }
                return Addressables.ResourceManager.CreateCompletedOperation<IList<T>>( result, null );
            } );
        }
        //public static AsyncOperationHandle<IList<T>> LoadPrefabListAsync<T>(IResourceLocation location) where T : Component {
        //    var handle = Addressables.LoadAssetsAsync<GameObject>( location, null! );
        //    return Addressables.ResourceManager.CreateChainOperation<IList<T>, IList<GameObject>>( handle, i => {
        //        var result = new List<T>( i.Result.Count );
        //        foreach (var item in i.Result) {
        //            var item_ = (T?) item.GetComponent<T>();
        //            if (item_ != null) {
        //                result.Add( item_ );
        //            } else {
        //                return Addressables.ResourceManager.CreateCompletedOperation<IList<T>>( null!, $"Component '{typeof( T )}' was not found" );
        //            }
        //        }
        //        return Addressables.ResourceManager.CreateCompletedOperation<IList<T>>( result, null );
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

    }
}

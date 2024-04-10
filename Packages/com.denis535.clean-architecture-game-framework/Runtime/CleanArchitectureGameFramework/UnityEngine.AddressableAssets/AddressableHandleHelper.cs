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

        // LoadAssetListAsync
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadAssetListAsync<T>(string[] keys) where T : UnityEngine.Object {
            var handle = Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union );
            return Addressables.ResourceManager.CreateChainOperation<IReadOnlyList<T>, IList<T>>( handle, i => Addressables.ResourceManager.CreateCompletedOperation( (IReadOnlyList<T>) handle.Result, null ) );
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
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : Component {
            var handle = Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union );
            return Addressables.ResourceManager.CreateChainOperation<IReadOnlyList<T>, IList<GameObject>>( handle, i => {
                var result = new List<T>( i.Result.Count );
                foreach (var item in i.Result) {
                    var item_ = (T?) item.GetComponent<T>();
                    if (item_ != null) {
                        result.Add( item_ );
                    } else {
                        return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( null!, $"Component '{typeof( T )}' was not found" );
                    }
                }
                return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( result, null );
            } );
        }
        //public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(IResourceLocation location) where T : Component {
        //    var handle = Addressables.LoadAssetsAsync<GameObject>( location, null! );
        //    return Addressables.ResourceManager.CreateChainOperation<IReadOnlyList<T>, IList<GameObject>>( handle, i => {
        //        var result = new List<T>( i.Result.Count );
        //        foreach (var item in i.Result) {
        //            var item_ = (T?) item.GetComponent<T>();
        //            if (item_ != null) {
        //                result.Add( item_ );
        //            } else {
        //                return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( null!, $"Component '{typeof( T )}' was not found" );
        //            }
        //        }
        //        return Addressables.ResourceManager.CreateCompletedOperation<IReadOnlyList<T>>( result, null );
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
        public static void Assert_IsValid<T>(this AddressableHandle<T> handle) {
            Assert.Operation.Message( $"AddressableHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this AddressableHandle<T> handle) {
            Assert.Operation.Message( $"AddressableHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this AddressableHandle<T> handle) {
            Assert.Operation.Message( $"AddressableHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

        // Assert
        public static void Assert_IsValid<T>(this DynamicAddressableHandle<T> handle) {
            Assert.Operation.Message( $"AddressableDynamicHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this DynamicAddressableHandle<T> handle) {
            Assert.Operation.Message( $"AssetHAddressableDynamicHandleandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this DynamicAddressableHandle<T> handle) {
            Assert.Operation.Message( $"AddressableDynamicHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

        // Assert
        public static void Assert_IsValid<T>(this AddressableListHandle<T> handle) {
            Assert.Operation.Message( $"AddressableHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this AddressableListHandle<T> handle) {
            Assert.Operation.Message( $"AddressableHandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this AddressableListHandle<T> handle) {
            Assert.Operation.Message( $"AddressableHandle {handle} is already valid" ).Valid( !handle.IsValid );
        }

        // Assert
        public static void Assert_IsValid<T>(this DynamicAddressableListHandle<T> handle) {
            Assert.Operation.Message( $"AddressableDynamicHandle {handle} must be valid" ).Valid( handle.IsValid );
        }
        public static void Assert_IsSucceeded<T>(this DynamicAddressableListHandle<T> handle) {
            Assert.Operation.Message( $"AssetHAddressableDynamicHandleandle {handle} must be succeeded" ).Valid( handle.IsSucceeded );
        }
        public static void Assert_IsNotValid<T>(this DynamicAddressableListHandle<T> handle) {
            Assert.Operation.Message( $"AddressableDynamicHandle {handle} is already valid" ).Valid( !handle.IsValid );
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

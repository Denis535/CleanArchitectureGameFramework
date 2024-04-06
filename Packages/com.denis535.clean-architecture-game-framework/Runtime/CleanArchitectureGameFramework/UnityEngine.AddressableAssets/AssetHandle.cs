#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AssetHandle<T> where T : notnull, UnityEngine.Object {

        private AsyncOperationHandle<T>? handle;

        public string Key { get; }
        public bool IsActive => handle != null;
        public bool IsValid => handle != null && handle.Value.IsValid();
        public bool IsSucceeded => handle != null && handle.Value.IsValid() && handle.Value.IsSucceeded();
        public bool IsFailed => handle != null && handle.Value.IsValid() && handle.Value.IsFailed();
        public T Asset {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( handle != null );
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( handle.Value.IsValid() );
                Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( handle.Value.IsSucceeded() );
                return handle.Value.Result;
            }
        }

        // Constructor
        public AssetHandle(string key) {
            Key = key;
        }

        // Load
        public T Load() {
            // what if release was called?
            if (handle == null) {
                handle = Addressables.LoadAssetAsync<T>( Key );
            }
            if (handle.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = handle.Value.WaitForCompletion();
                if (handle.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw handle.Value.OperationException;
        }

        // LoadAsync
        public async Task<T> LoadAsync(CancellationToken cancellationToken) {
            // what if release was called?
            if (handle == null) {
                handle = Addressables.LoadAssetAsync<T>( Key );
            }
            if (handle.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await handle.Value.Task.WaitAsync( cancellationToken );
                if (handle.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw handle.Value.OperationException;
        }
        public void Release() {
            if (handle != null) {
                Addressables.Release( handle.Value );
                handle = null;
            }
        }

        // Utils
        public override string ToString() {
            return $"AssetHandle: {Key}";
        }

    }
    public static class AssetHandleExtensions {

        public static void Release<T>(this IEnumerable<AssetHandle<T>> collection) where T : notnull, UnityEngine.Object {
            foreach (var item in collection) {
                item.Release();
            }
        }

    }
}

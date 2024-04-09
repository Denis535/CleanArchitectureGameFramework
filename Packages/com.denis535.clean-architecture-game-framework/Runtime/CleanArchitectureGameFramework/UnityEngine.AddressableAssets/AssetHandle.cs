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

        private AsyncOperationHandle<T> asset;

        public bool IsValid => asset.IsValid();
        public bool IsSucceeded => asset.IsValid() && asset.IsSucceeded();
        public bool IsFailed => asset.IsValid() && asset.IsFailed();
        public T Asset {
            get {
                Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.IsValid() );
                Assert.Operation.Message( $"AssetHandle {this} must be succeeded" ).Valid( asset.IsSucceeded() );
                return asset.Result;
            }
        }

        // Constructor
        public AssetHandle() {
        }

        // LoadAssetAsync
        public Task<T> LoadAssetAsync(string key, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !asset.IsValid() );
            asset = Addressables.LoadAssetAsync<T>( key );
            return GetAssetAsync( cancellationToken );
        }
        public async Task<T> GetAssetAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.IsValid() );
            if (asset.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await asset.Task.WaitAsync( cancellationToken );
                if (asset.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw asset.OperationException;
        }
        public void Release() {
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.IsValid() );
            Addressables.Release( asset );
            asset = default;
        }
        public void ReleaseSafe() {
            Addressables.Release( asset );
            asset = default;
        }

        // Utils
        public override string ToString() {
            return asset.DebugName;
        }

    }
}

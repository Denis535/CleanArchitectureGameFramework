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

    public class AssetHandle<T> where T : notnull, UnityEngine.Object {

        private AsyncOperationHandle<T>? asset;

        public string Key { get; }
        public bool IsActive => asset != null;
        public bool IsValid => asset != null && asset.Value.IsValid();
        public bool IsSucceeded => asset != null && asset.Value.IsValid() && asset.Value.IsSucceeded();
        public bool IsFailed => asset != null && asset.Value.IsValid() && asset.Value.IsFailed();
        public T Asset {
            get {
                Assert.Operation.Message( $"AssetHandle {this} must be active" ).Valid( asset != null );
                Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.Value.IsValid() );
                Assert.Operation.Message( $"AssetHandle {this} must be succeeded" ).Valid( asset.Value.IsSucceeded() );
                return asset.Value.Result;
            }
        }

        // Constructor
        public AssetHandle(string key) {
            Key = key;
        }

        // LoadAssetAsync
        public Task<T> LoadAssetAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} must be non-active" ).Valid( asset == null );
            asset = Addressables.LoadAssetAsync<T>( Key );
            return GetAssetAsync( cancellationToken );
        }
        public async Task<T> GetAssetAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} must be active" ).Valid( asset != null );
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.Value.IsValid() );
            if (asset.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await asset.Value.Task.WaitAsync( cancellationToken ).ConfigureAwait( false );
                if (asset.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw asset.Value.OperationException;
        }
        public void Release() {
            Assert.Operation.Message( $"AssetHandle {this} must be active" ).Valid( asset != null );
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.Value.IsValid() );
            Addressables.Release( asset.Value );
            asset = null;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class AssetHandleExtensions {

        public static void ReleaseAll<T>(this IEnumerable<AssetHandle<T>> collection) where T : notnull, UnityEngine.Object {
            foreach (var item in collection.Where( i => i.IsActive )) {
                item.Release();
            }
        }

    }
}

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

        private AsyncOperationHandle<T> asset;
        private CancellationTokenRegistration releaseCancellationTokenRegistration;

        public string Key { get; }
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
        public AssetHandle(string key) {
            Key = key;
        }

        // LoadAssetAsync
        public Task<T> LoadAssetAsync(CancellationToken releaseCancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} already exists" ).Valid( !asset.IsValid() );
            Assert.Operation.Message( $"AssetHandle {this} already exists" ).Valid( releaseCancellationTokenRegistration == default );
            asset = Addressables.LoadAssetAsync<T>( Key );
            releaseCancellationTokenRegistration = releaseCancellationToken.Register( () => Release() );
            return GetAssetAsync( releaseCancellationToken );
        }
        public async Task<T> GetAssetAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.IsValid() );
            if (asset.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await asset.Task.WaitAsync( cancellationToken ).ConfigureAwait( false );
                if (asset.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw asset.OperationException;
        }
        public void Release() {
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.IsValid() );
            {
                Addressables.Release( asset );
                asset = default;
                releaseCancellationTokenRegistration.Dispose();
                releaseCancellationTokenRegistration = default;
            }
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class AssetHandleExtensions {

        public static void ReleaseAll<T>(this IEnumerable<AssetHandle<T>> collection) where T : notnull, UnityEngine.Object {
            foreach (var item in collection.Where( i => i.IsValid )) {
                item.Release();
            }
        }

    }
}

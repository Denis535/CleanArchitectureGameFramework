#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetHandleBase<T> where T : notnull, UnityEngine.Object {

        protected AsyncOperationHandle<T> asset;

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
        public AssetHandleBase() {
        }

        // LoadAssetAsync
        protected Task<T> LoadAssetAsync(AsyncOperationHandle<T> asset, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !this.asset.IsValid() );
            this.asset = asset;
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
            if (asset.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return asset.DebugName;
        }

    }
    // AssetHandle
    public class AssetHandle<T> : AssetHandleBase<T> where T : notnull, UnityEngine.Object {

        public string Key { get; }

        // Constructor
        public AssetHandle(string key) {
            Key = key;
        }

        // LoadAssetAsync
        public Task<T> LoadAssetAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !asset.IsValid() );
            return LoadAssetAsync( Addressables.LoadAssetAsync<T>( Key ), cancellationToken );
        }

    }
    // DynamicAssetHandle
    public class DynamicAssetHandle<T> : AssetHandleBase<T> where T : notnull, UnityEngine.Object {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( asset.IsValid() );
                return key!;
            }
            private set {
                key = value;
            }
        }

        // Constructor
        public DynamicAssetHandle() {
        }

        // LoadAssetAsync
        public Task<T> LoadAssetAsync(string key, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !asset.IsValid() );
            return LoadAssetAsync( Addressables.LoadAssetAsync<T>( Key = key ), cancellationToken );
        }

    }
}

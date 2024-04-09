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

        protected AsyncOperationHandle<T> AssetHandle { get; private set; }
        public bool IsValid => AssetHandle.IsValid();
        public bool IsSucceeded => AssetHandle.IsValid() && AssetHandle.IsSucceeded();
        public bool IsFailed => AssetHandle.IsValid() && AssetHandle.IsFailed();
        public T Asset {
            get {
                Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( AssetHandle.IsValid() );
                Assert.Operation.Message( $"AssetHandle {this} must be succeeded" ).Valid( AssetHandle.IsSucceeded() );
                return AssetHandle.Result;
            }
        }

        // Constructor
        public AssetHandleBase() {
        }

        // LoadAssetAsync
        protected Task<T> LoadAssetAsync(AsyncOperationHandle<T> asset, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !AssetHandle.IsValid() );
            AssetHandle = asset;
            return GetAssetAsync( cancellationToken );
        }
        public async Task<T> GetAssetAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( AssetHandle.IsValid() );
            if (AssetHandle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await AssetHandle.Task.WaitAsync( cancellationToken );
                if (AssetHandle.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw AssetHandle.OperationException;
        }
        public void Release() {
            Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( AssetHandle.IsValid() );
            Addressables.Release( AssetHandle );
            AssetHandle = default;
        }
        public void ReleaseSafe() {
            if (AssetHandle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return AssetHandle.DebugName;
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
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !AssetHandle.IsValid() );
            return LoadAssetAsync( Addressables.LoadAssetAsync<T>( Key ), cancellationToken );
        }

    }
    // DynamicAssetHandle
    public class DynamicAssetHandle<T> : AssetHandleBase<T> where T : notnull, UnityEngine.Object {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert.Operation.Message( $"AssetHandle {this} must be valid" ).Valid( AssetHandle.IsValid() );
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
            Assert.Operation.Message( $"AssetHandle {this} is already valid" ).Valid( !AssetHandle.IsValid() );
            return LoadAssetAsync( Addressables.LoadAssetAsync<T>( Key = key ), cancellationToken );
        }

    }
}

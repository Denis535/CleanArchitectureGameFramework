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
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return AssetHandle.Result;
            }
        }

        // Constructor
        public AssetHandleBase() {
        }

        // Initialize
        //protected void Initialize(IResourceLocation location) {
        //    this.Assert_IsNotValid();
        //    AssetHandle = Addressables.LoadAssetAsync<T>( location );
        //}
        protected void Initialize(AsyncOperationHandle<T> handle) {
            this.Assert_IsNotValid();
            AssetHandle = handle;
        }

        // GetAssetAsync
        public async Task<T> GetAssetAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            return await AssetHandle.GetResultAsync( cancellationToken );
        }

        // Release
        public void Release() {
            this.Assert_IsValid();
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
            this.Assert_IsNotValid();
            Initialize( Addressables.LoadAssetAsync<T>( Key ) );
            return GetAssetAsync( cancellationToken );
        }

    }
    // DynamicAssetHandle
    public class DynamicAssetHandle<T> : AssetHandleBase<T> where T : notnull, UnityEngine.Object {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                this.Assert_IsValid();
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
            this.Assert_IsNotValid();
            Initialize( Addressables.LoadAssetAsync<T>( Key = key ) );
            return GetAssetAsync( cancellationToken );
        }

    }
}

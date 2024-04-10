#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetListHandleBase<T> where T : notnull, UnityEngine.Object {

        protected AsyncOperationHandle<IList<T>> AssetListHandle { get; private set; }
        public bool IsValid => AssetListHandle.IsValid();
        public bool IsSucceeded => AssetListHandle.IsValid() && AssetListHandle.IsSucceeded();
        public bool IsFailed => AssetListHandle.IsValid() && AssetListHandle.IsFailed();
        public IReadOnlyList<T> AssetList {
            get {
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return (IReadOnlyList<T>) AssetListHandle.Result;
            }
        }

        // Constructor
        public AssetListHandleBase() {
        }

        // Initialize
        //protected void Initialize(IResourceLocation location) {
        //    this.Assert_IsNotValid();
        //    AssetListHandle = Addressables.LoadAssetAsync<IList<T>>( location );
        //}
        protected void Initialize(AsyncOperationHandle<IList<T>> handle) {
            this.Assert_IsNotValid();
            AssetListHandle = handle;
        }

        // GetAssetListAsync
        public async Task<IReadOnlyList<T>> GetAssetListAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            return (IReadOnlyList<T>) await AssetListHandle.GetResultAsync( cancellationToken );
        }

        // Release
        public void Release() {
            this.Assert_IsValid();
            Addressables.Release( AssetListHandle );
            AssetListHandle = default;
        }
        public void ReleaseSafe() {
            if (AssetListHandle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return AssetListHandle.DebugName;
        }

    }
    // AssetListHandle
    public class AssetListHandle<T> : AssetListHandleBase<T> where T : notnull, UnityEngine.Object {

        public string[] Keys { get; }

        // Constructor
        public AssetListHandle(string[] keys) {
            Keys = keys;
        }

        // LoadAssetListAsync
        public Task<IReadOnlyList<T>> LoadAssetListAsync(CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Initialize( Addressables.LoadAssetsAsync<T>( Keys.AsEnumerable(), null, Addressables.MergeMode.Union ) );
            return GetAssetListAsync( cancellationToken );
        }

    }
    // DynamicAssetListHandle
    public class DynamicAssetListHandle<T> : AssetListHandleBase<T> where T : notnull, UnityEngine.Object {

        private string[]? keys;

        [AllowNull]
        public string[] Keys {
            get {
                this.Assert_IsValid();
                return keys!;
            }
            private set {
                keys = value;
            }
        }

        // Constructor
        public DynamicAssetListHandle() {
        }

        // LoadAssetListAsync
        public Task<IReadOnlyList<T>> LoadAssetListAsync(string[] keys, CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Initialize( Addressables.LoadAssetsAsync<T>( (Keys = keys).AsEnumerable(), null, Addressables.MergeMode.Union ) );
            return GetAssetListAsync( cancellationToken );
        }

    }
}

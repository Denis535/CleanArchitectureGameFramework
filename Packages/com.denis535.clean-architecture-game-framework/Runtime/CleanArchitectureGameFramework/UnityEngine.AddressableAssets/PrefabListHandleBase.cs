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

    public abstract class PrefabListHandleBase<T> where T : notnull, Component {

        protected AsyncOperationHandle<IList<T>> PrefabListHandle { get; private set; }
        public bool IsValid => PrefabListHandle.IsValid();
        public bool IsSucceeded => PrefabListHandle.IsValid() && PrefabListHandle.IsSucceeded();
        public bool IsFailed => PrefabListHandle.IsValid() && PrefabListHandle.IsFailed();
        public IReadOnlyList<T> PrefabList {
            get {
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return (IReadOnlyList<T>) PrefabListHandle.Result;
            }
        }

        // Constructor
        public PrefabListHandleBase() {
        }

        // Initialize
        //protected void Initialize(IResourceLocation location) {
        //    this.Assert_IsNotValid();
        //    PrefabListHandle = AddressableHandleHelper.LoadPrefabListAsync<T>( location );
        //}
        protected void Initialize(AsyncOperationHandle<IList<T>> handle) {
            this.Assert_IsNotValid();
            PrefabListHandle = handle;
        }

        // GetPrefabListAsync
        public async Task<IReadOnlyList<T>> GetPrefabListAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            return (IReadOnlyList<T>) await PrefabListHandle.GetResultAsync( cancellationToken );
        }

        // Release
        public void Release() {
            this.Assert_IsValid();
            Addressables.Release( PrefabListHandle );
            PrefabListHandle = default;
        }
        public void ReleaseSafe() {
            if (PrefabListHandle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return PrefabListHandle.DebugName;
        }

    }
    // PrefabListHandle
    public class PrefabListHandle<T> : PrefabListHandleBase<T> where T : notnull, Component {

        public string[] Keys { get; }

        // Constructor
        public PrefabListHandle(string[] keys) {
            Keys = keys;
        }

        // LoadPrefabAsync
        public async Task<IReadOnlyList<T>> LoadPrefabAsync(CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Initialize( AddressableHandleHelper.LoadPrefabListAsync<T>( Keys ) );
            return await GetPrefabListAsync( cancellationToken );
        }

    }
    // DynamicPrefabListHandle
    public class DynamicPrefabListHandle<T> : PrefabListHandleBase<T> where T : notnull, Component {

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
        public DynamicPrefabListHandle() {
        }

        // LoadPrefabAsync
        public async Task<IReadOnlyList<T>> LoadPrefabAsync(string[] keys, CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Initialize( AddressableHandleHelper.LoadPrefabListAsync<T>( Keys = keys ) );
            return await GetPrefabListAsync( cancellationToken );
        }

    }
}

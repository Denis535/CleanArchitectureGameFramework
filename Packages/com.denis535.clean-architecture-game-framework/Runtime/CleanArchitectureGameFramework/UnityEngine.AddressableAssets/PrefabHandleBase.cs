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

    public abstract class PrefabHandleBase<T> where T : notnull, Component {

        protected AsyncOperationHandle<T> PrefabHandle { get; private set; }
        public bool IsValid => PrefabHandle.IsValid();
        public bool IsSucceeded => PrefabHandle.IsValid() && PrefabHandle.IsSucceeded();
        public bool IsFailed => PrefabHandle.IsValid() && PrefabHandle.IsFailed();
        public T Prefab {
            get {
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return PrefabHandle.Result;
            }
        }

        // Constructor
        public PrefabHandleBase() {
        }

        // Initialize
        //protected void Initialize(IResourceLocation location) {
        //    this.Assert_IsNotValid();
        //    PrefabHandle = AddressableHandleHelper.LoadPrefabAsync<T>( location );
        //}
        protected void Initialize(AsyncOperationHandle<T> handle) {
            this.Assert_IsNotValid();
            PrefabHandle = handle;
        }

        // GetPrefabAsync
        public Task<T> GetPrefabAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            return PrefabHandle.GetResultAsync( cancellationToken );
        }

        // Release
        public void Release() {
            this.Assert_IsValid();
            Addressables.Release( PrefabHandle );
            PrefabHandle = default;
        }
        public void ReleaseSafe() {
            if (PrefabHandle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return PrefabHandle.DebugName;
        }

    }
    // PrefabHandle
    public class PrefabHandle<T> : PrefabHandleBase<T> where T : notnull, Component {

        public string Key { get; }

        // Constructor
        public PrefabHandle(string key) {
            Key = key;
        }

        // LoadPrefabAsync
        public Task<T> LoadPrefabAsync(CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Initialize( AddressableHandleHelper.LoadPrefabAsync<T>( Key ) );
            return GetPrefabAsync( cancellationToken );
        }

    }
    // DynamicPrefabHandle
    public class DynamicPrefabHandle<T> : PrefabHandleBase<T> where T : notnull, Component {

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
        public DynamicPrefabHandle() {
        }

        // LoadPrefabAsync
        public Task<T> LoadPrefabAsync(string key, CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Initialize( AddressableHandleHelper.LoadPrefabAsync<T>( Key = key ) );
            return GetPrefabAsync( cancellationToken );
        }

    }
}

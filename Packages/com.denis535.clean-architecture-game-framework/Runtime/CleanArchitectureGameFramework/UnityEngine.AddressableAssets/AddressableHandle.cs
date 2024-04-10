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

    public abstract class AddressableHandle<T> {

        protected AsyncOperationHandle<T> Handle { get; set; }
        public string Key { get; }
        public bool IsValid => Handle.IsValid();
        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        public T Result {
            get {
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return Handle.Result;
            }
        }

        // Constructor
        public AddressableHandle(string key) {
            Key = key;
        }

        // LoadAsync
        public abstract Task<T> LoadAsync(CancellationToken cancellationToken);

        // GetResultAsync
        public async Task<T> GetResultAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            return await Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public void Release() {
            this.Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
        public void ReleaseSafe() {
            if (Handle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

    }
    public abstract class DynamicAddressableHandle<T> {

        private string? key;

        protected AsyncOperationHandle<T> Handle { get; set; }
        [AllowNull]
        public string Key {
            get {
                this.Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }
        public bool IsValid => Handle.IsValid();
        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        public T Result {
            get {
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return Handle.Result;
            }
        }

        // Constructor
        public DynamicAddressableHandle() {
        }

        // LoadAsync
        public abstract Task<T> LoadAsync(string key, CancellationToken cancellationToken);

        // GetResultAsync
        public async Task<T> GetResultAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            return await Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public void Release() {
            this.Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
        public void ReleaseSafe() {
            if (Handle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

    }
}

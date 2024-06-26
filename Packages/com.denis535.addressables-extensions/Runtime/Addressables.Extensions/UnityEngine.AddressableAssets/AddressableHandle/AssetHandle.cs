#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetHandle : AddressableHandle {

        // Constructor
        public AssetHandle() {
        }

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

        // GetValue
        public abstract UnityEngine.Object GetValueBase();
        public abstract ValueTask<UnityEngine.Object> GetValueBaseAsync(CancellationToken cancellationToken);

        // Release
        public abstract void Release();
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return "AssetHandle: " + Key;
        }

    }
    public class AssetHandle<T> : AssetHandle, IAssetHandle<AssetHandle<T>, T> where T : notnull, UnityEngine.Object {

        // Key
        public override string Key { get; }
        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsValid() && Handle.IsDone;
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();

        // Constructor
        public AssetHandle(string key) {
            Key = key;
        }
        public AssetHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // Load
        public AssetHandle<T> Load() {
            Assert_IsNotValid();
            Handle = Addressables2.LoadAssetAsync<T>( Key );
            return this;
        }

        // Wait
        public override void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public override ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.WaitAsync( cancellationToken );
        }

        // GetValue
        public override UnityEngine.Object GetValueBase() {
            return GetValue();
        }
        public override async ValueTask<UnityEngine.Object> GetValueBaseAsync(CancellationToken cancellationToken) {
            return await GetValueAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public override void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }

    }
}

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AssetHandle<T> : AddressableHandle<T> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetHandle(string key) : base( key ) {
        }

        // Load
        public AssetHandle<T> Load() {
            Assert_IsNotValid();
            Handle = Addressables2.LoadAssetAsync<T>( Key );
            return this;
        }

        // Wait
        public void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.WaitAsync( cancellationToken );
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
        public void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
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
}

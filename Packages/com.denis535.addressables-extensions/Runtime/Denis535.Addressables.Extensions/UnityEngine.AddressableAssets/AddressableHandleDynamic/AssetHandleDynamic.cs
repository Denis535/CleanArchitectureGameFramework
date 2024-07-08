#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AssetHandleDynamic<T> : AddressableHandleDynamic<AssetHandle<T>>, IAssetHandle<AssetHandleDynamic<T>, T> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetHandleDynamic() {
        }

        // SetUp
        public new AssetHandleDynamic<T> SetUp(AssetHandle<T>? handle) {
            return (AssetHandleDynamic<T>) base.SetUp( handle );
        }

        // Load
        public AssetHandleDynamic<T> Load() {
            Assert_HasHandle();
            Handle.Load();
            return this;
        }

        // Wait
        public void Wait() {
            Assert_HasHandle();
            Handle.Wait();
        }
        public ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_HasHandle();
            return Handle.WaitAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_HasHandle();
            return Handle.GetValue();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_HasHandle();
            return Handle.GetValueAsync( cancellationToken );
        }

        // Release
        public void Release() {
            Assert_HasHandle();
            Handle.Release();
        }
        public void ReleaseSafe() {
            if (Handle != null && Handle.IsValid) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            if (Handle != null) {
                return "AssetHandleDynamic: " + Handle.Key;
            } else {
                return "AssetHandleDynamic";
            }
        }

    }
}

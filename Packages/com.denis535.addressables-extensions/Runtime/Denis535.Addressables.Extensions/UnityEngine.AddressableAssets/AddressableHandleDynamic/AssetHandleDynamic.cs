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

        // SetHandle
        public AssetHandleDynamic<T> SetHandle(string? key) {
            if (key != null) {
                return (AssetHandleDynamic<T>) base.SetHandle( new AssetHandle<T>( key ) );
            } else {
                return (AssetHandleDynamic<T>) base.SetHandle( null );
            }
        }
        public new AssetHandleDynamic<T> SetHandle(AssetHandle<T>? handle) {
            return (AssetHandleDynamic<T>) base.SetHandle( handle );
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

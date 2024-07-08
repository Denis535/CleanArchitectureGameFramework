#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AssetListHandleDynamic<T> : AddressableListHandleDynamic<AssetListHandle<T>>, IAssetListHandle<AssetListHandleDynamic<T>, T> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetListHandleDynamic() {
        }

        // SetUp
        public new AssetListHandleDynamic<T> SetUp(AssetListHandle<T>? handle) {
            return (AssetListHandleDynamic<T>) base.SetUp( handle );
        }

        // Load
        public AssetListHandleDynamic<T> Load() {
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
        public IReadOnlyList<T> GetValues() {
            Assert_HasHandle();
            return Handle.GetValues();
        }
        public ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_HasHandle();
            return Handle.GetValuesAsync( cancellationToken );
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
                return "AssetListHandleDynamic: " + string.Join( ", ", Handle.Keys );
            } else {
                return "AssetListHandleDynamic";
            }
        }

    }
}

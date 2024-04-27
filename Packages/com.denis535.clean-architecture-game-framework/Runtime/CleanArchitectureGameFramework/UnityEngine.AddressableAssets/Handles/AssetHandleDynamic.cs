#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetHandleDynamic<T> : AddressableHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new AssetHandle<T> Handle => (AssetHandle<T>) base.Handle;

        // Constructor
        public AssetHandleDynamic() {
        }

        // SetHandle
        public AssetHandle<T> SetHandle(string key) {
            Assert_IsNotValid();
            base.handle = new AssetHandle<T>( key );
            return Handle;
        }
        public AssetHandle<T> SetHandle(AssetHandle handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return Handle;
        }

    }
}

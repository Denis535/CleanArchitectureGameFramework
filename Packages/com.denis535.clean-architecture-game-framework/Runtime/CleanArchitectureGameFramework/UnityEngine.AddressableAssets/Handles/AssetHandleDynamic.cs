#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetHandleDynamic<T> : AddressableHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new AssetHandle? Handle => (AssetHandle?) base.Handle;

        // Constructor
        public AssetHandleDynamic() {
        }

        // SetAsset
        public AssetHandleDynamic<T> SetAsset(string key) {
            Assert_IsNotValid();
            base.Handle = new AssetHandle<T>( key );
            return this;
        }
        public AssetHandleDynamic<T> SetAsset(AssetHandle handle) {
            Assert_IsNotValid();
            base.Handle = handle;
            return this;
        }

    }
}

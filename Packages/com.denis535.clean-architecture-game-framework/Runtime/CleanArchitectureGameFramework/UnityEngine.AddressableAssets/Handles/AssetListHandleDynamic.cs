#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetListHandleDynamic<T> : AddressableListHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new AssetListHandle<T>? Handle => (AssetListHandle<T>?) base.Handle;

        // Constructor
        public AssetListHandleDynamic() {
        }

        // SetAssetList
        public AssetListHandleDynamic<T> SetAssetList(string[] keys) {
            Assert_IsNotValid();
            base.handle = new AssetListHandle<T>( keys );
            return this;
        }
        public AssetListHandleDynamic<T> SetAssetList(AssetListHandle<T> handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return this;
        }

    }
}

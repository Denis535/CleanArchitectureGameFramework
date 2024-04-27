#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetListHandleDynamic<T> : AddressableListHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new AssetListHandle<T> Handle => (AssetListHandle<T>) base.Handle;

        // Constructor
        public AssetListHandleDynamic() {
        }

        // SetHandle
        public AssetListHandleDynamic<T> SetHandle(string[] keys) {
            Assert_IsNotValid();
            base.handle = new AssetListHandle<T>( keys );
            return this;
        }
        public AssetListHandleDynamic<T> SetHandle(AssetListHandle<T> handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return this;
        }

    }
}

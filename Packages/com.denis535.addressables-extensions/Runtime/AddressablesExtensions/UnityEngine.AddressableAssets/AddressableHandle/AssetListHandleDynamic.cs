#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetListHandleDynamic<T> : AddressableListHandleDynamic where T : notnull, UnityEngine.Object {

        private AssetListHandle<T>? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public AssetListHandle<T> Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public AssetListHandleDynamic() {
        }

        // SetUp
        public AssetListHandle<T> SetUp(string[] keys) {
            Assert_IsNotValid();
            return this.handle = new AssetListHandle<T>( keys );
        }
        public AssetListHandle<T> SetUp(AssetListHandle<T> handle) {
            Assert_IsNotValid();
            return this.handle = handle;
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "AssetListHandleDynamic: " + string.Join( ", ", Handle.Keys );
            } else {
                return "AssetListHandleDynamic";
            }
        }

    }
}

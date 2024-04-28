#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetHandleDynamic<T> : AddressableHandleDynamic where T : notnull, UnityEngine.Object {

        private AssetHandle<T>? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public AssetHandle<T> Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public AssetHandleDynamic() {
        }

        // SetUp
        public AssetHandle<T> SetUp(string key) {
            Assert_IsNotValid();
            return this.handle = new AssetHandle<T>( key );
        }
        public AssetHandle<T> SetUp(AssetHandle<T> handle) {
            Assert_IsNotValid();
            return this.handle = handle;
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "AssetHandleDynamic: " + Handle.Key;
            } else {
                return "AssetHandleDynamic";
            }
        }

    }
}

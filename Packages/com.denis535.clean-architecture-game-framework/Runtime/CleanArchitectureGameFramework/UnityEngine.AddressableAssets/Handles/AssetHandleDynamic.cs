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
        public override AddressableHandle HandleBase {
            get {
                Assert_IsValid();
                return handle!;
            }
        }
        public AssetHandle<T> Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public AssetHandleDynamic() {
        }

        // SetHandle
        public AssetHandle<T> SetHandle(string key) {
            Assert_IsNotValid();
            return this.handle = new AssetHandle<T>( key );
        }
        public AssetHandle<T> SetHandle(AssetHandle<T> handle) {
            Assert_IsNotValid();
            return this.handle = handle;
        }

    }
}

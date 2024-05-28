#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AssetHandleDynamic : AddressableHandleDynamic {

        protected AssetHandle? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public AssetHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public AssetHandleDynamic() {
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
    public class AssetHandleDynamic<T> : AssetHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new AssetHandle<T> Handle => (AssetHandle<T>) base.Handle;

        // Constructor
        public AssetHandleDynamic() {
        }

        // SetUp
        public AssetHandle<T> SetUp(string key) {
            Assert_IsNotValid();
            base.handle = new AssetHandle<T>( key );
            return (AssetHandle<T>) base.handle;
        }
        public AssetHandle<T> SetUp(AssetHandle<T> handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return (AssetHandle<T>) base.handle;
        }

    }
}

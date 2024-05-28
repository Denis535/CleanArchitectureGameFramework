#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AssetListHandleDynamic : AddressableListHandleDynamic {

        protected AssetListHandle? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public AssetListHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public AssetListHandleDynamic() {
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
    public class AssetListHandleDynamic<T> : AssetListHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new AssetListHandle<T> Handle => (AssetListHandle<T>) base.Handle;

        // Constructor
        public AssetListHandleDynamic() {
        }

        // SetUp
        public AssetListHandle<T> SetUp(string[] keys) {
            Assert_IsNotValid();
            base.handle = new AssetListHandle<T>( keys );
            return (AssetListHandle<T>) base.handle;
        }
        public AssetListHandle<T> SetUp(AssetListHandle<T> handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return (AssetListHandle<T>) base.handle;
        }

    }
}

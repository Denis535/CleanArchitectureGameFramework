#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PrefabListHandleDynamic : AddressableListHandleDynamic {

        protected PrefabListHandle? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public PrefabListHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public PrefabListHandleDynamic() {
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "PrefabListHandleDynamic: " + string.Join( ", ", Handle.Keys );
            } else {
                return "PrefabListHandleDynamic";
            }
        }

    }
    public class PrefabListHandleDynamic<T> : PrefabListHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new PrefabListHandle<T> Handle => (PrefabListHandle<T>) base.Handle;

        // Constructor
        public PrefabListHandleDynamic() {
        }

        // SetUp
        public PrefabListHandle<T> SetUp(string[] keys) {
            Assert_IsNotValid();
            this.handle = new PrefabListHandle<T>( keys );
            return (PrefabListHandle<T>) base.Handle;
        }
        public PrefabListHandle<T> SetUp(PrefabListHandle<T> handle) {
            Assert_IsNotValid();
            this.handle = handle;
            return (PrefabListHandle<T>) base.Handle;
        }

    }
}

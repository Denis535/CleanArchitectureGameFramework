#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PrefabHandleDynamic : AddressableHandleDynamic {

        protected PrefabHandle? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public PrefabHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public PrefabHandleDynamic() {
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "PrefabHandleDynamic: " + Handle.Key;
            } else {
                return "PrefabHandleDynamic";
            }
        }

    }
    public class PrefabHandleDynamic<T> : PrefabHandleDynamic where T : notnull, UnityEngine.Object {

        // Handle
        public new PrefabHandle<T> Handle => (PrefabHandle<T>) base.Handle;

        // Constructor
        public PrefabHandleDynamic() {
        }

        // SetUp
        public PrefabHandle<T> SetUp(string key) {
            Assert_IsNotValid();
            base.handle = new PrefabHandle<T>( key );
            return (PrefabHandle<T>) base.Handle;
        }
        public PrefabHandle<T> SetUp(PrefabHandle<T> handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return (PrefabHandle<T>) base.Handle;
        }

    }
}

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SceneHandleDynamic : AddressableHandleDynamic {

        // Handle
        public new SceneHandle Handle => (SceneHandle) base.Handle;

        // Constructor
        public SceneHandleDynamic() {
        }

        // SetHandle
        public SceneHandleDynamic SetHandle(string key) {
            Assert_IsNotValid();
            base.handle = new SceneHandle( key );
            return this;
        }
        public SceneHandleDynamic SetHandle(SceneHandle handle) {
            Assert_IsNotValid();
            base.handle = handle;
            return this;
        }

    }
}

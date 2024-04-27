#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SceneHandleDynamic : AddressableHandleDynamic {

        private SceneHandle? handle;

        // IsValid
        public override bool IsValid => handle != null && handle.IsValid;
        // Handle
        public override AddressableHandle HandleBase {
            get {
                Assert_IsValid();
                return handle!;
            }
        }
        public SceneHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public SceneHandleDynamic() {
        }

        // SetHandle
        public SceneHandle SetHandle(string key) {
            Assert_IsNotValid();
            return this.handle = new SceneHandle( key );
        }
        public SceneHandle SetHandle(SceneHandle handle) {
            Assert_IsNotValid();
            return this.handle = handle;
        }

    }
}

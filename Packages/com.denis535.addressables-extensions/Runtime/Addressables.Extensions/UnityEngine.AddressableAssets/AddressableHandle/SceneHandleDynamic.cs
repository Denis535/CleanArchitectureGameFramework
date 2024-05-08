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
        public SceneHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }

        // Constructor
        public SceneHandleDynamic() {
        }

        // SetUp
        public SceneHandle SetUp(string key) {
            Assert_IsNotValid();
            return this.handle = new SceneHandle( key );
        }
        public SceneHandle SetUp(SceneHandle handle) {
            Assert_IsNotValid();
            return this.handle = handle;
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "SceneHandleDynamic: " + Handle.Key;
            } else {
                return "SceneHandleDynamic";
            }
        }

    }
}

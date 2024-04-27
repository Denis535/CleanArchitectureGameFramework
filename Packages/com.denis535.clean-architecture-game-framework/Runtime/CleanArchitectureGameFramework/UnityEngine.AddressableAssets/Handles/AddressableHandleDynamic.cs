#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandleDynamic {

        protected AddressableHandle? handle;

        // Handle
        public AddressableHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }
        public bool IsValid => Handle != null && Handle.IsValid;

        // Constructor
        public AddressableHandleDynamic() {
        }

        // Utils
        public override string ToString() {
            if (Handle != null) {
                return "AddressableHandleDynamic: " + Handle.Key;
            } else {
                return "AddressableHandleDynamic";
            }
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandleDynamic {this} must be valid" ).Valid( IsValid );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandleDynamic {this} is already valid" ).Valid( !IsValid );
        }

    }
}

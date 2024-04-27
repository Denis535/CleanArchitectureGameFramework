#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableListHandleDynamic {

        protected AddressableListHandle? handle;

        // Handle
        public AddressableListHandle Handle {
            get {
                Assert_IsValid();
                return handle!;
            }
        }
        public bool IsValid => Handle != null && Handle.IsValid;

        // Constructor
        public AddressableListHandleDynamic() {
        }

        // Utils
        public override string ToString() {
            if (Handle != null) {
                return "AddressableListHandleDynamic: " + string.Join( ", ", Handle.Keys );
            } else {
                return "AddressableListHandleDynamic";
            }
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableListHandleDynamic {this} must be valid" ).Valid( IsValid );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableListHandleDynamic {this} is already valid" ).Valid( !IsValid );
        }

    }
}

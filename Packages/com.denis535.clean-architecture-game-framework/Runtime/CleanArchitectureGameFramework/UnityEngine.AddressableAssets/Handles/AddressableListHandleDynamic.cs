#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableListHandleDynamic {

        // Handle
        public AddressableListHandle? Handle { get; protected set; }

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
            Assert.Operation.Message( $"AddressableListHandleDynamic {this} must be valid" ).Valid( Handle != null && Handle.IsValid );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableListHandleDynamic {this} is already valid" ).Valid( Handle == null || !Handle.IsValid );
        }

    }
}

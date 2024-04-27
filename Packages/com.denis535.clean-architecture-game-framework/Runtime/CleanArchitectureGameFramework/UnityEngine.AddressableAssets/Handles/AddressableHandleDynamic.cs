#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandleDynamic {

        // Handle
        public AddressableHandle? Handle { get; protected set; }

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
            Assert.Operation.Message( $"AddressableHandleDynamic {this} must be valid" ).Valid( Handle != null && Handle.IsValid );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandleDynamic {this} is already valid" ).Valid( Handle == null || !Handle.IsValid );
        }

    }
}

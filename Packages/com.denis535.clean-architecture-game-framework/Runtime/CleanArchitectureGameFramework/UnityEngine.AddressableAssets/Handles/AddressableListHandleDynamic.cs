#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableListHandleDynamic {

        // IsValid
        public abstract bool IsValid { get; }
        // Handle
        public abstract AddressableListHandle HandleBase { get; }

        // Constructor
        public AddressableListHandleDynamic() {
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "AddressableListHandleDynamic: " + string.Join( ", ", HandleBase.Keys );
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

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandleDynamic {

        // IsValid
        public abstract bool IsValid { get; }

        // Constructor
        public AddressableHandleDynamic() {
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

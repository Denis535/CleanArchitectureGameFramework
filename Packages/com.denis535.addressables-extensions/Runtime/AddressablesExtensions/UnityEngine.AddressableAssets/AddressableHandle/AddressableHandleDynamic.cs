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
            Debug.Assert( IsValid, $"AddressableHandleDynamic {this} must be valid" );
        }
        protected void Assert_IsNotValid() {
            Debug.Assert( !IsValid, $"AddressableHandleDynamic {this} is already valid" );
        }

    }
}

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableListHandleDynamic {

        // IsValid
        public abstract bool IsValid { get; }

        // Constructor
        public AddressableListHandleDynamic() {
        }

        // Heleprs
        protected void Assert_IsValid() {
            if (IsValid) return;
            throw new InvalidOperationException( $"AddressableListHandleDynamic `{this}` must be valid" );
        }
        protected void Assert_IsNotValid() {
            if (!IsValid) return;
            throw new InvalidOperationException( $"AddressableListHandleDynamic `{this}` is already valid" );
        }

    }
}

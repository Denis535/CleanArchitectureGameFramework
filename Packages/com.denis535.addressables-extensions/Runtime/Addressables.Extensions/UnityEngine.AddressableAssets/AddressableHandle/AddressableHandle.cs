#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandle {

        // Key
        public string Key { get; }
        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }

        // Constructor
        public AddressableHandle(string key) {
            Key = key;
        }

        // Heleprs
        protected void Assert_IsValid() {
            if (IsValid) return;
            throw new InvalidOperationException( $"AddressableHandle `{this}` must be valid" );
        }
        protected void Assert_IsSucceeded() {
            if (IsSucceeded) return;
            throw new InvalidOperationException( $"AddressableHandle `{this}` must be succeeded" );
        }
        protected void Assert_IsNotValid() {
            if (!IsValid) return;
            throw new InvalidOperationException( $"AddressableHandle `{this}` is already valid" );
        }

    }
}

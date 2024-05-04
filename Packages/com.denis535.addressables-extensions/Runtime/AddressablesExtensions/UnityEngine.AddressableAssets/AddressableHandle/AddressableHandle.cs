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
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableHandle(string key) {
            Key = key;
        }

        // Heleprs
        protected void Assert_IsValid() {
            Debug.Assert( IsValid, $"AddressableHandle {this} must be valid" );
        }
        protected void Assert_IsSucceeded() {
            Debug.Assert( IsSucceeded, $"AddressableHandle {this} must be succeeded" );
        }
        protected void Assert_IsNotValid() {
            Debug.Assert( !IsValid, $"AddressableHandle {this} is already valid" );
        }

    }
}

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableListHandle {

        // Keys
        public string[] Keys { get; }
        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableListHandle(string[] keys) {
            Keys = keys;
        }

        // Heleprs
        protected void Assert_IsValid() {
            Debug.Assert( IsValid, $"AddressableListHandle {this} must be valid" );
        }
        protected void Assert_IsSucceeded() {
            Debug.Assert( IsSucceeded, $"AddressableListHandle {this} must be succeeded" );
        }
        protected void Assert_IsNotValid() {
            Debug.Assert( !IsValid, $"AddressableListHandle {this} is already valid" );
        }

    }
}

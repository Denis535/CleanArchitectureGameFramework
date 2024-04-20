#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandle {

        // Key
        public string? Key { get; protected set; }
        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableHandle(string? key) {
            Key = key;
        }

        // Utils
        public override string ToString() {
            if (Key != null) {
                return "AddressableHandle: " + Key;
            } else {
                return "AddressableHandle";
            }
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandle {this} must be valid" ).Valid( IsValid );
        }
        protected void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableHandle {this} must be succeeded" ).Valid( IsSucceeded );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandle {this} is already valid" ).Valid( !IsValid );
        }

    }
    public abstract class AddressableListHandle {

        // Keys
        public string[]? Keys { get; protected set; }
        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableListHandle(string[]? keys) {
            Keys = keys;
        }

        // Utils
        public override string ToString() {
            if (Keys != null) {
                return "AddressableListHandle: " + string.Join( ", ", Keys );
            } else {
                return "AddressableListHandle";
            }
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandle {this} must be valid" ).Valid( IsValid );
        }
        protected void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableHandle {this} must be succeeded" ).Valid( IsSucceeded );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandle {this} is already valid" ).Valid( !IsValid );
        }

    }
}

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandleBase {

        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableHandleBase() {
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
    public abstract class AddressableListHandleBase {

        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableListHandleBase() {
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableListHandle {this} must be valid" ).Valid( IsValid );
        }
        protected void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableListHandle {this} must be succeeded" ).Valid( IsSucceeded );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableListHandle {this} is already valid" ).Valid( !IsValid );
        }

    }
}

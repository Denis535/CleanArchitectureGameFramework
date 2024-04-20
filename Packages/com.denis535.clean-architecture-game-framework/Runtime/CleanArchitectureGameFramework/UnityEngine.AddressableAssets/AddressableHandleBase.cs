#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableHandleBase {

        // State
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        // Exception
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableHandleBase() {
        }

    }
    public abstract class AddressableHandleBase<T> : AddressableHandleBase {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        // Exception
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public AddressableHandleBase() {
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandle {this} must be valid" ).Valid( Handle.IsValid() );
        }
        protected void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableHandle {this} must be succeeded" ).Valid( Handle.IsSucceeded() );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandle {this} is already valid" ).Valid( !Handle.IsValid() );
        }

    }
}

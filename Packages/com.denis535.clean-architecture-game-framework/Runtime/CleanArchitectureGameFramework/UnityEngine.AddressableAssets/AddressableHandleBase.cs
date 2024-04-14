#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
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
    public abstract class AddressableHandleBase<T> : AddressableHandleBase where T : notnull {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        // Value
        public T Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public T? ValueSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }
        // Exception
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public AddressableHandleBase() {
        }

        // GetValueAsync
        public async ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
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
    public abstract class AddressableListHandleBase<T> : AddressableHandleBase where T : notnull {

        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        // Values
        public IReadOnlyList<T> Values {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public IReadOnlyList<T>? ValuesSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }
        // Exception
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public AddressableListHandleBase() {
        }

        // GetValuesAsync
        public async ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
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

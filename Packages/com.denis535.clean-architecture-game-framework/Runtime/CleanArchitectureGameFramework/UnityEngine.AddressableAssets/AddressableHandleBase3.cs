#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableHandleBase3<T> : AddressableHandleBase2 where T : notnull {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        // Exception
        public override Exception? Exception => Handle.OperationException;
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

        // Constructor
        public AddressableHandleBase3(string key) : base( key ) {
        }
        public AddressableHandleBase3(string key, AsyncOperationHandle<T> handle) : base( key ) {
            Handle = handle;
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

    }
    public abstract class AddressableListHandleBase3<T> : AddressableListHandleBase2 where T : notnull {

        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        // Exception
        public override Exception? Exception => Handle.OperationException;
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

        // Constructor
        public AddressableListHandleBase3(string[] keys) : base( keys ) {
        }
        public AddressableListHandleBase3(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) : base( keys ) {
            Handle = handle;
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

    }
    public abstract class DynamicAddressableHandleBase3<T> : DynamicAddressableHandleBase2 where T : notnull {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        // Exception
        public override Exception? Exception => Handle.OperationException;
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

        // Constructor
        public DynamicAddressableHandleBase3() {
        }
        public DynamicAddressableHandleBase3(string key, AsyncOperationHandle<T> handle) : base( key ) {
            Key = key;
            Handle = handle;
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

    }
    public abstract class DynamicAddressableListHandleBase3<T> : DynamicAddressableListHandleBase2 where T : notnull {

        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        // Exception
        public override Exception? Exception => Handle.OperationException;
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

        // Constructor
        public DynamicAddressableListHandleBase3() {
        }
        public DynamicAddressableListHandleBase3(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
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

    }
}

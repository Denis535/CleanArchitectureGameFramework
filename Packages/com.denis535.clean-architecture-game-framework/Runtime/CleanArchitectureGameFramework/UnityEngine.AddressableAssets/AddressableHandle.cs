#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableHandle<T> : AddressableHandleBase<T> where T : notnull {

        // Key
        public string Key { get; }
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
        public AddressableHandle(string key) {
            Key = key;
        }
        public AddressableHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // GetValueAsync
        public async ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

    }
    public abstract class AddressableListHandle<T> : AddressableHandleBase<IReadOnlyList<T>> where T : notnull {

        // Keys
        public string[] Keys { get; }
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
        public AddressableListHandle(string[] keys) {
            Keys = keys;
        }
        public AddressableListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // GetValuesAsync
        public async ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

    }
    public abstract class DynamicAddressableHandle<T> : AddressableHandleBase<T> where T : notnull {

        private string? key;

        // Key
        [AllowNull]
        public string Key {
            get {
                Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }
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
        public DynamicAddressableHandle() {
        }
        public DynamicAddressableHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // GetValueAsync
        public async ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

    }
    public abstract class DynamicAddressableListHandle<T> : AddressableHandleBase<IReadOnlyList<T>> where T : notnull {

        private string[]? keys;

        // Keys
        [AllowNull]
        public string[] Keys {
            get {
                Assert_IsValid();
                return keys!;
            }
            protected set {
                keys = value;
            }
        }
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
        public DynamicAddressableListHandle() {
        }
        public DynamicAddressableListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // GetValuesAsync
        public async ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

    }
}

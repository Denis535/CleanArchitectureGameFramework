#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableHandle<T> : AddressableHandleBase<T> where T : notnull {

        // Key
        public string Key { get; }

        // Constructor
        public AddressableHandle(string key) {
            Key = key;
        }
        public AddressableHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

    }
    public abstract class AddressableListHandle<T> : AddressableListHandleBase<T> where T : notnull {

        // Keys
        public string[] Keys { get; }

        // Constructor
        public AddressableListHandle(string[] keys) {
            Keys = keys;
        }
        public AddressableListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
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

        // Constructor
        public DynamicAddressableHandle() {
        }
        public DynamicAddressableHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

    }
    public abstract class DynamicAddressableListHandle<T> : AddressableListHandleBase<T> where T : notnull {

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

        // Constructor
        public DynamicAddressableListHandle() {
        }
        public DynamicAddressableListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

    }
}

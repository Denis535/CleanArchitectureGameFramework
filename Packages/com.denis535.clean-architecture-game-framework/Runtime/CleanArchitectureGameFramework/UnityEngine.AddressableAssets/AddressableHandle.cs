#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableHandle<T> {

        protected AsyncOperationHandle<T> Handle { get; set; }
        public bool IsValid => Handle.IsValid();
        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        public T Result {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }

        // Constructor
        public AddressableHandle() {
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
    public abstract class AddressableHandle<T, TKey> : AddressableHandle<T> where T : notnull where TKey : notnull {

        public TKey Key { get; }

        // Constructor
        public AddressableHandle(TKey key) {
            Key = key;
        }

    }
    public abstract class DynamicAddressableHandle<T, TKey> : AddressableHandle<T> where T : notnull where TKey : notnull {

        private TKey? key;

        [AllowNull]
        public TKey Key {
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
            Key = default;
        }

    }
}

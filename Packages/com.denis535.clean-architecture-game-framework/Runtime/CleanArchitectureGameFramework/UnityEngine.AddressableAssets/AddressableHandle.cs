#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableHandle<T, TKey> where T : notnull where TKey : notnull {

        protected AsyncOperationHandle<T> Handle { get; set; }
        public TKey Key { get; }
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
        public AddressableHandle(TKey key) {
            Key = key;
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
    public abstract class DynamicAddressableHandle<T, TKey> where T : notnull where TKey : notnull {

        private TKey? key;

        protected AsyncOperationHandle<T> Handle { get; set; }
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
        public DynamicAddressableHandle() {
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"DynamicAddressableHandle {this} must be valid" ).Valid( Handle.IsValid() );
        }
        protected void Assert_IsSucceeded() {
            Assert.Operation.Message( $"DynamicAddressableHandle {this} must be succeeded" ).Valid( Handle.IsSucceeded() );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"DynamicAddressableHandle {this} is already valid" ).Valid( !Handle.IsValid() );
        }

    }
}

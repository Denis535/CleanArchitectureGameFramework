#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AddressableListHandle {

        // Keys
        public string[] Keys { get; }
        // Handle
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }

        // Constructor
        public AddressableListHandle(string[] keys) {
            Keys = keys;
        }

        // Heleprs
        protected void Assert_IsValid() {
            if (IsValid) return;
            throw new InvalidOperationException( $"AddressableListHandle `{this}` must be valid" );
        }
        protected void Assert_IsNotValid() {
            if (!IsValid) return;
            throw new InvalidOperationException( $"AddressableListHandle `{this}` is already valid" );
        }

    }
    public abstract class AddressableListHandle<T> : AddressableListHandle where T : notnull {

        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsValid() && Handle.IsDone;
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();

        // Constructor
        public AddressableListHandle(string[] keys) : base( keys ) {
        }

    }
}

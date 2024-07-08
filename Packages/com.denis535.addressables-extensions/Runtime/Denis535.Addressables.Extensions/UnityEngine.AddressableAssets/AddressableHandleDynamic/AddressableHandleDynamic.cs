#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    public abstract class AddressableHandleDynamic {

        // Handle
        public abstract bool HasHandle { get; }
        public abstract string Key { get; }
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }

        // Constructor
        public AddressableHandleDynamic() {
        }

    }
    public abstract class AddressableHandleDynamic<THandle> : AddressableHandleDynamic where THandle : notnull, AddressableHandle {

        // Handle
        public THandle? Handle { get; private set; }
        [MemberNotNullWhen( true, "Handle" )] public override bool HasHandle => Handle != null;
        public override string Key {
            get {
                Assert_HasHandle();
                return Handle.Key;
            }
        }
        public override bool IsValid {
            get {
                Assert_HasHandle();
                return Handle.IsValid;
            }
        }
        public override bool IsDone {
            get {
                Assert_HasHandle();
                return Handle.IsDone;
            }
        }
        public override bool IsSucceeded {
            get {
                Assert_HasHandle();
                return Handle.IsSucceeded;
            }
        }
        public override bool IsFailed {
            get {
                Assert_HasHandle();
                return Handle.IsFailed;
            }
        }

        // Constructor
        public AddressableHandleDynamic() {
        }

        // SetHandle
        protected AddressableHandleDynamic<THandle> SetHandle(THandle? handle) {
            if (Handle != null && Handle.IsValid) throw new InvalidOperationException( $"AddressableHandleDynamic `{this}` already has valid '{Handle}' handle" );
            Handle = handle;
            return this;
        }

        // Heleprs
        [MemberNotNull( "Handle" )]
        protected void Assert_HasHandle() {
            if (HasHandle) return;
            throw new InvalidOperationException( $"AddressableHandleDynamic `{this}` must have handle" );
        }

    }
}

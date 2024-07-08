#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    public abstract class AddressableListHandleDynamic {

        // Handle
        public abstract bool HasHandle { get; }
        public abstract string[] Keys { get; }
        public abstract bool IsValid { get; }
        public abstract bool IsDone { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }

        // Constructor
        public AddressableListHandleDynamic() {
        }

    }
    public abstract class AddressableListHandleDynamic<THandle> : AddressableListHandleDynamic where THandle : notnull, AddressableListHandle {

        // Handle
        public THandle? Handle { get; private set; }
        [MemberNotNullWhen( true, "Handle" )] public override bool HasHandle => Handle != null;
        public override string[] Keys {
            get {
                Assert_HasHandle();
                return Handle.Keys;
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
        public AddressableListHandleDynamic() {
        }

        // SetHandle
        protected AddressableListHandleDynamic<THandle> SetHandle(THandle? handle) {
            if (Handle != null && Handle.IsValid) throw new InvalidOperationException( $"AddressableListHandleDynamic `{this}` already has valid '{Handle}' handle" );
            Handle = handle;
            return this;
        }

        // Heleprs
        [MemberNotNull( "Handle" )]
        protected void Assert_HasHandle() {
            if (HasHandle) return;
            throw new InvalidOperationException( $"AddressableListHandleDynamic `{this}` must have handle" );
        }

    }
}

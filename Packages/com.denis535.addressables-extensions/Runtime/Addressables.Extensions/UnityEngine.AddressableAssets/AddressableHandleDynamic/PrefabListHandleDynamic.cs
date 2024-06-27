#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PrefabListHandleDynamic<T> : AddressableListHandleDynamic, IPrefabListHandle<PrefabListHandleDynamic<T>, T> where T : notnull, UnityEngine.Object {

        // Handle
        [MemberNotNullWhen( true, "Handle" )]
        public override bool IsValid {
            get {
                return Handle != null;
            }
        }
        public PrefabListHandle<T>? Handle { get; private set; }
        public override string[] Keys {
            get {
                Assert_IsValid();
                return Handle!.Keys;
            }
        }
        public override bool IsDone {
            get {
                Assert_IsValid();
                return Handle!.IsDone;
            }
        }
        public override bool IsSucceeded {
            get {
                Assert_IsValid();
                return Handle!.IsSucceeded;
            }
        }
        public override bool IsFailed {
            get {
                Assert_IsValid();
                return Handle!.IsFailed;
            }
        }

        // Constructor
        public PrefabListHandleDynamic() {
        }

        // SetUp
        public PrefabListHandleDynamic<T> SetUp(string[] keys) {
            Assert_IsNotValid();
            Handle = new PrefabListHandle<T>( keys );
            return this;
        }
        public PrefabListHandleDynamic<T> SetUp(PrefabListHandle<T> handle) {
            Assert_IsNotValid();
            Handle = handle;
            return this;
        }

        // Load
        public PrefabListHandleDynamic<T> Load() {
            Assert_IsValid();
            Handle!.Load();
            return this;
        }

        // Wait
        public void Wait() {
            Assert_IsValid();
            Handle!.Wait();
        }
        public ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle!.WaitAsync( cancellationToken );
        }

        // GetValue
        public IReadOnlyList<UnityEngine.Object> GetValuesBase() {
            Assert_IsValid();
            return Handle!.GetValuesBase();
        }
        public ValueTask<IReadOnlyList<UnityEngine.Object>> GetValuesBaseAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle!.GetValuesBaseAsync( cancellationToken );
        }

        // GetValue
        public IReadOnlyList<T> GetValues() {
            Assert_IsValid();
            return Handle!.GetValues();
        }
        public ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle!.GetValuesAsync( cancellationToken );
        }

        // Release
        public void Release() {
            Assert_IsValid();
            Handle!.Release();
            Handle = null;
        }
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "PrefabListHandleDynamic: " + string.Join( ", ", Keys );
            } else {
                return "PrefabListHandleDynamic";
            }
        }

    }
}

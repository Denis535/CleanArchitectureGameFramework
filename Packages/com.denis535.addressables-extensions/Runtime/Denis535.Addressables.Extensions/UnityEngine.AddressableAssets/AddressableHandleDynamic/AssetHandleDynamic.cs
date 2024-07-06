#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AssetHandleDynamic<T> : AddressableHandleDynamic, IAssetHandle<AssetHandleDynamic<T>, T> where T : notnull, UnityEngine.Object {

        // Handle
        [MemberNotNullWhen( true, "Handle" )]
        public override bool IsValid {
            get {
                return Handle != null;
            }
        }
        public AssetHandle<T>? Handle { get; private set; }
        public override string Key {
            get {
                Assert_IsValid();
                return Handle!.Key;
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
        public AssetHandleDynamic() {
        }

        // SetUp
        public AssetHandleDynamic<T> SetUp(string key) {
            Assert_IsNotValid();
            Handle = new AssetHandle<T>( key );
            return this;
        }
        public AssetHandleDynamic<T> SetUp(AssetHandle<T> handle) {
            Assert_IsNotValid();
            Handle = handle;
            return this;
        }

        // Load
        public AssetHandleDynamic<T> Load() {
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
        public UnityEngine.Object GetValueBase() {
            Assert_IsValid();
            return Handle!.GetValueBase();
        }
        public ValueTask<UnityEngine.Object> GetValueBaseAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle!.GetValueBaseAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_IsValid();
            return Handle!.GetValue();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle!.GetValueAsync( cancellationToken );
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
                return "AssetHandleDynamic: " + Key;
            } else {
                return "AssetHandleDynamic";
            }
        }

    }
}

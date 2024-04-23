#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetHandle : AddressableHandle {

        // Constructor
        public AssetHandle(string key) : base( key ) {
        }

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

        // Release
        public abstract void Release();
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

    }
    public abstract class DynamicAssetHandle : DynamicAddressableHandle {

        // Constructor
        public DynamicAssetHandle(string? key) : base( key ) {
        }

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

        // Release
        public abstract void Release();
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

    }
    public class AssetHandle<T> : AssetHandle where T : notnull, UnityEngine.Object {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
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
        public AssetHandle(string key) : base( key ) {
        }
        public AssetHandle(string key, AsyncOperationHandle<T> handle) : base( key ) {
            Handle = handle;
        }

        // Load
        public AssetHandle<T> Load() {
            Assert_IsNotValid();
            Handle = AddressableHelper.LoadAssetAsync<T>( Key );
            return this;
        }

        // Wait
        public override void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public override ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.WaitAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public override void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }

    }
    public class DynamicAssetHandle<T> : DynamicAssetHandle where T : notnull, UnityEngine.Object {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
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
        public DynamicAssetHandle() : base( null ) {
        }
        public DynamicAssetHandle(string? key, AsyncOperationHandle<T> handle) : base( key ) {
            Handle = handle;
        }

        // Load
        public DynamicAssetHandle<T> Load(string key) {
            Assert_IsNotValid();
            Handle = AddressableHelper.LoadAssetAsync<T>( Key = key );
            return this;
        }

        // Wait
        public override void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public override ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.WaitAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public override void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Key = null;
            Handle = default;
        }

    }
}

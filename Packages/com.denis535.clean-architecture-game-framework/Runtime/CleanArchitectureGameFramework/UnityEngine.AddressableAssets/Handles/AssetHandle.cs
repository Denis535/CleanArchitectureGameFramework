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

        // Value
        public abstract UnityEngine.Object ValueBase { get; }
        public abstract UnityEngine.Object? ValueBaseSafe { get; }

        // Constructor
        public AssetHandle(string key) : base( key ) {
        }

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

        // GetValue
        public abstract UnityEngine.Object GetValueBase();
        public abstract ValueTask<UnityEngine.Object> GetValueBaseAsync(CancellationToken cancellationToken);

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
        private AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public override UnityEngine.Object ValueBase {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public T Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public override UnityEngine.Object? ValueBaseSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
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
        public override UnityEngine.Object GetValueBase() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public T GetValue() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public override async ValueTask<UnityEngine.Object> GetValueBaseAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return await Handle.GetResultAsync( cancellationToken );
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
}

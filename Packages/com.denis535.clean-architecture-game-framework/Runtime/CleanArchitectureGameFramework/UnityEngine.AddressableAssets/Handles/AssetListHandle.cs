#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetListHandle : AddressableListHandle {

        // Constructor
        public AssetListHandle(string[] keys) : base( keys ) {
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
    public abstract class DynamicAssetListHandle : DynamicAddressableListHandle {

        // Constructor
        public DynamicAssetListHandle(string[]? keys) : base( keys ) {
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
    public class AssetListHandle<T> : AssetListHandle where T : notnull, UnityEngine.Object {

        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public IReadOnlyList<T> Values {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public IReadOnlyList<T>? ValuesSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }

        // Constructor
        public AssetListHandle(string[] keys) : base( keys ) {
        }
        public AssetListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) : base( keys ) {
            Handle = handle;
        }

        // Load
        public AssetListHandle<T> Load() {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Keys );
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

        // GetValues
        public IReadOnlyList<T> GetValues() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
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
    public class DynamicAssetListHandle<T> : DynamicAssetListHandle where T : notnull, UnityEngine.Object {

        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public IReadOnlyList<T> Values {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public IReadOnlyList<T>? ValuesSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }

        // Constructor
        public DynamicAssetListHandle() : base( null ) {
        }
        public DynamicAssetListHandle(string[]? keys, AsyncOperationHandle<IReadOnlyList<T>> handle) : base( keys ) {
            Handle = handle;
        }

        // Load
        public DynamicAssetListHandle<T> Load(string[] keys) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Keys = keys );
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

        // GetValues
        public IReadOnlyList<T> GetValues() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public override void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Keys = null;
            Handle = default;
        }

    }
}

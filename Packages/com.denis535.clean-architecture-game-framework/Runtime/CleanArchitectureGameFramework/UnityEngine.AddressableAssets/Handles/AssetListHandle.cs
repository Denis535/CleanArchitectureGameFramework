#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetListHandle : AddressableListHandle {

        // Values
        public abstract IReadOnlyList<UnityEngine.Object> ValuesBase { get; }
        public abstract IReadOnlyList<UnityEngine.Object>? ValuesBaseSafe { get; }

        // Constructor
        public AssetListHandle(string[] keys) : base( keys ) {
        }

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

        // GetValues
        public abstract IReadOnlyList<UnityEngine.Object> GetValuesBase();
        public abstract ValueTask<IReadOnlyList<UnityEngine.Object>> GetValuesBaseAsync(CancellationToken cancellationToken);

        // Release
        public abstract void Release();
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return "AssetListHandle: " + string.Join( ", ", Keys );
        }

    }
    public class AssetListHandle<T> : AssetListHandle where T : notnull, UnityEngine.Object {

        // Handle
        private AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        // Values
        public override IReadOnlyList<UnityEngine.Object> ValuesBase {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public override IReadOnlyList<UnityEngine.Object>? ValuesBaseSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }
        // Values
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
            Handle = AddressableHelper.LoadAssetListAsync<T>( Keys );
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
        public override IReadOnlyList<UnityEngine.Object> GetValuesBase() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public override async ValueTask<IReadOnlyList<UnityEngine.Object>> GetValuesBaseAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return await Handle.GetResultAsync( cancellationToken );
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
}

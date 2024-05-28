#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
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
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsFailed();
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public AssetListHandle(string[] keys) : base( keys ) {
        }
        public AssetListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) : base( keys ) {
            Handle = handle;
        }

        // Load
        public AssetListHandle<T> Load() {
            Assert_IsNotValid();
            Handle = Addressables2.LoadAssetListAsync<T>( Keys );
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
            return GetValues();
        }
        public override async ValueTask<IReadOnlyList<UnityEngine.Object>> GetValuesBaseAsync(CancellationToken cancellationToken) {
            return await GetValuesAsync( cancellationToken );
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

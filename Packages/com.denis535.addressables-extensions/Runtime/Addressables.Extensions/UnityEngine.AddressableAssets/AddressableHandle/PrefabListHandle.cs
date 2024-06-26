#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class PrefabListHandle : AddressableListHandle {

        // Constructor
        public PrefabListHandle() {
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
            return "PrefabListHandle: " + string.Join( ", ", Keys );
        }

    }
    public class PrefabListHandle<T> : PrefabListHandle, IPrefabListHandle<PrefabListHandle<T>, T> where T : notnull, UnityEngine.Object {

        // Keys
        public override string[] Keys { get; }
        // Handle
        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsValid() && Handle.IsDone;
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();

        // Constructor
        public PrefabListHandle(params string[] keys) {
            Keys = keys;
        }
        public PrefabListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // Load
        public PrefabListHandle<T> Load() {
            Assert_IsNotValid();
            Handle = Addressables2.LoadPrefabListAsync<T>( Keys );
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

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class PrefabListHandle<T> : AddressableListHandle<T> where T : notnull, UnityEngine.Object {

        // Constructor
        public PrefabListHandle(params string[] keys) : base( keys ) {
        }

        // Load
        public PrefabListHandle<T> Load() {
            Assert_IsNotValid();
            Handle = Addressables2.LoadPrefabListAsync<T>( Keys );
            return this;
        }

        // Wait
        public void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public ValueTask WaitAsync(CancellationToken cancellationToken) {
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
        public void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
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
}

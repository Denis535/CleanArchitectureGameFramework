#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class PrefabHandle : AddressableHandle {

        // Constructor
        public PrefabHandle(string key) : base( key ) {
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

        // Utils
        public override string ToString() {
            return "PrefabHandle: " + Key;
        }

    }
    public class PrefabHandle<T> : PrefabHandle where T : notnull, UnityEngine.Object {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsFailed();
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public PrefabHandle(string key) : base( key ) {
        }
        public PrefabHandle(string key, AsyncOperationHandle<T> handle) : base( key ) {
            Handle = handle;
        }

        // Load
        public PrefabHandle<T> Load() {
            Assert_IsNotValid();
            Handle = Addressables2.LoadPrefabAsync<T>( Key );
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
            return GetValue();
        }
        public override async ValueTask<UnityEngine.Object> GetValueBaseAsync(CancellationToken cancellationToken) {
            return await GetValueAsync( cancellationToken );
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
}

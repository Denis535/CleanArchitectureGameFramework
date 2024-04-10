#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PrefabHandle<T> : AddressableHandle<T, string> where T : notnull, Component {

        // Constructor
        public PrefabHandle(string key) : base( key ) {
        }

        // LoadAsync
        public Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }

        // GetResultAsync
        public Task<T> GetResultAsync(CancellationToken cancellationToken) {
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
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    public class PrefabListHandle<T> : AddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, Component {

        // Constructor
        public PrefabListHandle(string[] key) : base( key ) {
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }

        // GetResultAsync
        public Task<IReadOnlyList<T>> GetResultAsync(CancellationToken cancellationToken) {
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
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    public class DynamicPrefabHandle<T> : DynamicAddressableHandle<T, string> where T : notnull, Component {

        // Constructor
        public DynamicPrefabHandle() {
        }

        // LoadAsync
        public Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }

        // GetResultAsync
        public Task<T> GetResultAsync(CancellationToken cancellationToken) {
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
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    public class DynamicPrefabListHandle<T> : DynamicAddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, Component {

        // Constructor
        public DynamicPrefabListHandle() {
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(string[] key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }

        // GetResultAsync
        public Task<IReadOnlyList<T>> GetResultAsync(CancellationToken cancellationToken) {
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
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
}

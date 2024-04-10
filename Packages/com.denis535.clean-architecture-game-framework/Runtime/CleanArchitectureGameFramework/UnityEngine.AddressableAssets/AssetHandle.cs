#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AssetHandle<T> : AddressableHandle<T, string> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetHandle(string key) : base( key ) {
        }

        // LoadAsync
        public Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetAsync<T>( Key );
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
    public class AssetListHandle<T> : AddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetListHandle(string[] key) : base( key ) {
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Key );
            return GetResultAsync( cancellationToken );
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
    public class DynamicAssetHandle<T> : DynamicAddressableHandle<T, string> where T : notnull, UnityEngine.Object {

        // Constructor
        public DynamicAssetHandle() {
        }

        // LoadAsync
        public Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetAsync<T>( Key = key );
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
    public class DynamicAssetListHandle<T> : DynamicAddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, UnityEngine.Object {

        // Constructor
        public DynamicAssetListHandle() {
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(string[] key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Key = key );
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

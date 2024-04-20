#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AssetHandle<T> : AddressableHandleBase3<T> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetHandle(string key) : base( key ) {
        }
        public AssetHandle(string key, AsyncOperationHandle<T> handle) : base( key, handle ) {
        }

        // LoadAsync
        public ValueTask<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.LoadAssetAsync<T>( Key );
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
    public class AssetListHandle<T> : AddressableListHandleBase3<T> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetListHandle(string[] keys) : base( keys ) {
        }
        public AssetListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) : base( keys, handle ) {
        }

        // LoadAsync
        public ValueTask<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.LoadAssetListAsync<T>( Keys );
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
    public class DynamicAssetHandle<T> : DynamicAddressableHandleBase3<T> where T : notnull, UnityEngine.Object {

        // Constructor
        public DynamicAssetHandle() {
        }
        public DynamicAssetHandle(string key, AsyncOperationHandle<T> handle) : base( key, handle ) {
        }

        // LoadAsync
        public ValueTask<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.LoadAssetAsync<T>( Key = key );
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
    public class DynamicAssetListHandle<T> : DynamicAddressableListHandleBase3<T> where T : notnull, UnityEngine.Object {

        // Constructor
        public DynamicAssetListHandle() {
        }
        public DynamicAssetListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) : base( keys, handle ) {
        }

        // LoadAsync
        public ValueTask<IReadOnlyList<T>> LoadAsync(string[] keys, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.LoadAssetListAsync<T>( Keys = keys );
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

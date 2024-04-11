#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AssetHandle<T> : AddressableAssetHandle<T> where T : notnull, UnityEngine.Object {

        public string Key { get; }

        // Constructor
        public AssetHandle(string key) {
            Key = key;
        }
        public AssetHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // LoadAsync
        public Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class AssetListHandle<T> : AddressableAssetHandle<IReadOnlyList<T>> where T : notnull, UnityEngine.Object {

        public string[] Keys { get; }

        // Constructor
        public AssetListHandle(string[] keys) {
            Keys = keys;
        }
        public AssetListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Keys );
            return GetValueAsync( cancellationToken );
        }

    }
    public class DynamicAssetHandle<T> : AddressableAssetHandle<T> where T : notnull, UnityEngine.Object {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }

        // Constructor
        public DynamicAssetHandle() {
        }
        public DynamicAssetHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // LoadAsync
        public Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class DynamicAssetListHandle<T> : AddressableAssetHandle<IReadOnlyList<T>> where T : notnull, UnityEngine.Object {

        private string[]? keys;

        [AllowNull]
        public string[] Keys {
            get {
                Assert_IsValid();
                return keys!;
            }
            protected set {
                keys = value;
            }
        }

        // Constructor
        public DynamicAssetListHandle() {
        }
        public DynamicAssetListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(string[] keys, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Keys = keys );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
}

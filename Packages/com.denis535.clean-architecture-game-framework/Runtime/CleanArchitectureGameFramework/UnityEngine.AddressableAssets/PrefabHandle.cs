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

    public class PrefabHandle<T> : AddressablePrefabHandle<T> where T : notnull, Component {

        public string Key { get; }

        // Constructor
        public PrefabHandle(string key) {
            Key = key;
        }
        public PrefabHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // LoadAsync
        public Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class PrefabListHandle<T> : AddressablePrefabHandle<IReadOnlyList<T>> where T : notnull, Component {

        public string[] Keys { get; }

        // Constructor
        public PrefabListHandle(string[] keys) {
            Keys = keys;
        }
        public PrefabListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Keys );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabHandle<T> : AddressablePrefabHandle<T> where T : notnull, Component {

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
        public DynamicPrefabHandle() {
        }
        public DynamicPrefabHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
        }

        // LoadAsync
        public Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabListHandle<T> : AddressablePrefabHandle<IReadOnlyList<T>> where T : notnull, Component {

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
        public DynamicPrefabListHandle() {
        }
        public DynamicPrefabListHandle(string[] keys, AsyncOperationHandle<IReadOnlyList<T>> handle) {
            Keys = keys;
            Handle = handle;
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(string[] keys, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Keys = keys );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
}

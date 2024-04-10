#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PrefabHandle<T> : AddressablePrefabHandle<T> where T : notnull, Component {

        public string Key { get; }

        // Constructor
        public PrefabHandle(string key) {
            Key = key;
        }

        // LoadAsync
        public Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class PrefabListHandle<T> : AddressablePrefabHandle<IReadOnlyList<T>> where T : notnull, Component {

        public string[] Key { get; }

        // Constructor
        public PrefabListHandle(string[] key) {
            Key = key;
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Key );
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

        // LoadAsync
        public Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabListHandle<T> : AddressablePrefabHandle<IReadOnlyList<T>> where T : notnull, Component {

        private string[]? key;

        [AllowNull]
        public string[] Key {
            get {
                Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }

        // Constructor
        public DynamicPrefabListHandle() {
        }

        // LoadAsync
        public Task<IReadOnlyList<T>> LoadAsync(string[] key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }

    }
}

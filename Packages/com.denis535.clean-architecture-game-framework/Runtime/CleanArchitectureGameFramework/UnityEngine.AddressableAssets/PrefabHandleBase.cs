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
        public override Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class PrefabListHandle<T> : AddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, Component {

        // Constructor
        public PrefabListHandle(string[] key) : base( key ) {
        }

        // LoadAsync
        public override Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabHandle<T> : DynamicAddressableHandle<T, string> where T : notnull, Component {

        // Constructor
        public DynamicPrefabHandle() {
        }

        // LoadAsync
        public override Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key = key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabListHandle<T> : DynamicAddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, Component {

        // Constructor
        public DynamicPrefabListHandle() {
        }

        // LoadAsync
        public override Task<IReadOnlyList<T>> LoadAsync(string[] key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Key = key );
            return GetResultAsync( cancellationToken );
        }

    }
}

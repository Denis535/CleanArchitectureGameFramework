#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PrefabHandle<T> : AddressableHandle<T> where T : notnull, Component {

        // Constructor
        public PrefabHandle(string key) : base( key ) {
        }

        // LoadAsync
        public override Task<T> LoadAsync(CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class PrefabListHandle<T> : AddressableListHandle<T> where T : notnull, Component {

        // Constructor
        public PrefabListHandle(string[] keys) : base( keys ) {
        }

        // LoadAsync
        public override Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Keys );
            return GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabHandle<T> : DynamicAddressableHandle<T> where T : notnull, Component {

        // Constructor
        public DynamicPrefabHandle() {
        }

        // LoadAsync
        public override Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabAsync<T>( Key = key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class DynamicPrefabListHandle<T> : DynamicAddressableListHandle<T> where T : notnull, Component {

        // Constructor
        public DynamicPrefabListHandle() {
        }

        // LoadAsync
        public override Task<IReadOnlyList<T>> LoadAsync(string[] keys, CancellationToken cancellationToken) {
            this.Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadPrefabListAsync<T>( Keys = keys );
            return GetResultAsync( cancellationToken );
        }

    }
}

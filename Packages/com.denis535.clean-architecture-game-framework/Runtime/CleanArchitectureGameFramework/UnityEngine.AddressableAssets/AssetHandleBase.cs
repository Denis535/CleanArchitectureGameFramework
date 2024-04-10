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
        public override Task<T> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetAsync<T>( Key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class AssetListHandle<T> : AddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, UnityEngine.Object {

        // Constructor
        public AssetListHandle(string[] key) : base( key ) {
        }

        // LoadAsync
        public override Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class DynamicAssetHandle<T> : DynamicAddressableHandle<T, string> where T : notnull, UnityEngine.Object {

        // Constructor
        public DynamicAssetHandle() {
        }

        // LoadAsync
        public override Task<T> LoadAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetAsync<T>( Key = key );
            return GetResultAsync( cancellationToken );
        }

    }
    public class DynamicAssetListHandle<T> : DynamicAddressableHandle<IReadOnlyList<T>, string[]> where T : notnull, UnityEngine.Object {

        // Constructor
        public DynamicAssetListHandle() {
        }

        // LoadAsync
        public override Task<IReadOnlyList<T>> LoadAsync(string[] key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.LoadAssetListAsync<T>( Key = key );
            return GetResultAsync( cancellationToken );
        }

    }
}

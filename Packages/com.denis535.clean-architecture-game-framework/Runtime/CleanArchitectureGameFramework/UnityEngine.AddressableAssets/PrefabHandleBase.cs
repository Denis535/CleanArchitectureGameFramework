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
    using UnityEngine.ResourceManagement.ResourceProviders;

    public abstract class PrefabHandleBase<T> where T : notnull, Component {

        protected AsyncOperationHandle<T> instance;

        public bool IsValid => instance.IsValid();
        public bool IsSucceeded => instance.IsValid() && instance.IsSucceeded();
        public bool IsFailed => instance.IsValid() && instance.IsFailed();
        public T Instance {
            get {
                Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
                Assert.Operation.Message( $"PrefabHandle {this} must be succeeded" ).Valid( instance.IsSucceeded() );
                return instance.Result;
            }
        }

        // Constructor
        public PrefabHandleBase() {
        }

        // InstantiateAsync
        protected Task<T> InstantiateAsync(AsyncOperationHandle<T> instance, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !this.instance.IsValid() );
            this.instance = instance;
            return GetInstanceAsync( cancellationToken );
        }
        public async Task<T> GetInstanceAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
            if (instance.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await instance.Task.WaitAsync( cancellationToken );
                if (instance.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw instance.OperationException;
        }
        public void ReleaseInstance() {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
            Addressables.ReleaseInstance( instance );
            instance = default;
        }
        public void ReleaseInstanceSafe() {
            if (instance.IsValid()) {
                ReleaseInstance();
            }
        }

        // Utils
        public override string ToString() {
            return instance.DebugName;
        }

        // Helpers
        protected static AsyncOperationHandle<T> InstantiateAsync(string key, Vector3? position, Quaternion? rotation, Transform? parent) {
            var instance = Addressables.InstantiateAsync( key, new InstantiationParameters( position ?? Vector3.zero, rotation ?? Quaternion.identity, parent ) );
            return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( instance, i => {
                var result = (T?) i.Result.GetComponent<T>();
                if (result != null) {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( result, null );
                } else {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
            } );
        }

    }
    // PrefabHandle
    public class PrefabHandle<T> : PrefabHandleBase<T> where T : notnull, Component {

        public string Key { get; }

        // Constructor
        public PrefabHandle(string key) {
            Key = key;
        }

        // InstantiateAsync
        public Task<T> InstantiateAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !instance.IsValid() );
            return InstantiateAsync( InstantiateAsync( Key, null, null, null ), cancellationToken );
        }
        public Task<T> InstantiateAsync(Transform? parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !instance.IsValid() );
            return InstantiateAsync( InstantiateAsync( Key, null, null, parent ), cancellationToken );
        }
        public Task<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !instance.IsValid() );
            return InstantiateAsync( InstantiateAsync( Key, position, rotation, parent ), cancellationToken );
        }

    }
    // DynamicPrefabHandle
    public class DynamicPrefabHandle<T> : PrefabHandleBase<T> where T : notnull, Component {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
                return key!;
            }
            private set {
                key = value;
            }
        }

        // Constructor
        public DynamicPrefabHandle() {
        }

        // InstantiateAsync
        public Task<T> InstantiateAsync(string key, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !instance.IsValid() );
            return InstantiateAsync( InstantiateAsync( Key = key, null, null, null ), cancellationToken );
        }
        public Task<T> InstantiateAsync(string key, Transform? parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !instance.IsValid() );
            return InstantiateAsync( InstantiateAsync( Key = key, null, null, parent ), cancellationToken );
        }
        public Task<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !instance.IsValid() );
            return InstantiateAsync( InstantiateAsync( Key = key, position, rotation, parent ), cancellationToken );
        }

    }
}

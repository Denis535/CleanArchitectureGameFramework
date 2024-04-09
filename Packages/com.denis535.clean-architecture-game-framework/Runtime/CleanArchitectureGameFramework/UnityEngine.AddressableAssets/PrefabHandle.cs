#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class PrefabHandle<T> where T : notnull, Component {

        private AsyncOperationHandle<T> instance;

        public string Key { get; }
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
        public PrefabHandle(string key) {
            Key = key;
        }

        // InstantiateAsync
        public Task<T> InstantiateAsync(CancellationToken cancellationToken) {
            return InstantiateAsync( Vector3.zero, Quaternion.identity, null, cancellationToken );
        }
        public Task<T> InstantiateAsync(Transform? parent, CancellationToken cancellationToken) {
            return InstantiateAsync( Vector3.zero, Quaternion.identity, parent, cancellationToken );
        }
        public Task<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} already exists" ).Valid( !instance.IsValid() );
            instance = InstantiateAsync( Key, position, rotation, parent );
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
        public void Destroy() {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
            Addressables.ReleaseInstance( instance );
            instance = default;
        }
        public void DestroySafe() {
            Addressables.ReleaseInstance( instance );
            instance = default;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

        // Helpers
        private static AsyncOperationHandle<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform? parent) {
            var instance = Addressables.InstantiateAsync( key, new InstantiationParameters( position, rotation, parent ) );
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
}

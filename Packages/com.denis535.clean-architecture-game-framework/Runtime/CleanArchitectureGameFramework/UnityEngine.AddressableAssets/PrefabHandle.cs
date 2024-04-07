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

    public class PrefabHandle {

        private AsyncOperationHandle<GameObject> instance;
        private CancellationTokenRegistration destroyCancellationTokenRegistration;

        public string Key { get; }
        public bool IsValid => instance.IsValid();
        public bool IsSucceeded => instance.IsValid() && instance.IsSucceeded();
        public bool IsFailed => instance.IsValid() && instance.IsFailed();
        public GameObject Instance {
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
        public Task<GameObject> InstantiateAsync(CancellationToken destroyCancellationToken) {
            return InstantiateAsync( Vector3.zero, Quaternion.identity, null, destroyCancellationToken );
        }
        public Task<GameObject> InstantiateAsync(Transform? parent, CancellationToken destroyCancellationToken) {
            return InstantiateAsync( Vector3.zero, Quaternion.identity, parent, destroyCancellationToken );
        }
        public Task<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform? parent, CancellationToken destroyCancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} already exists" ).Valid( !instance.IsValid() );
            Assert.Operation.Message( $"PrefabHandle {this} already exists" ).Valid( destroyCancellationTokenRegistration == default );
            instance = Addressables.InstantiateAsync( Key, new InstantiationParameters( position, rotation, parent ) );
            destroyCancellationTokenRegistration = destroyCancellationToken.Register( () => Destroy() );
            return GetInstanceAsync( destroyCancellationToken );
        }
        public async Task<GameObject> GetInstanceAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
            if (instance.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await instance.Task.WaitAsync( cancellationToken ).ConfigureAwait( false );
                if (instance.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw instance.OperationException;
        }
        public void Destroy() {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.IsValid() );
            {
                Addressables.ReleaseInstance( instance );
                instance = default;
                destroyCancellationTokenRegistration.Dispose();
                destroyCancellationTokenRegistration = default;
            }
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class PrefabHandleExtensions {

        public static void DestroyAll<T>(this IEnumerable<PrefabHandle> collection) {
            foreach (var item in collection.Where( i => i.IsValid )) {
                item.Destroy();
            }
        }

    }
}

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

        private AsyncOperationHandle<GameObject>? instance;

        public string Key { get; }
        public bool IsActive => instance != null;
        public bool IsValid => instance != null && instance.Value.IsValid();
        public bool IsSucceeded => instance != null && instance.Value.IsValid() && instance.Value.IsSucceeded();
        public bool IsFailed => instance != null && instance.Value.IsValid() && instance.Value.IsFailed();
        public GameObject Instance {
            get {
                Assert.Operation.Message( $"PrefabHandle {this} must be active" ).Valid( instance != null );
                Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.Value.IsValid() );
                Assert.Operation.Message( $"PrefabHandle {this} must be succeeded" ).Valid( instance.Value.IsSucceeded() );
                return instance.Value.Result;
            }
        }

        // Constructor
        public PrefabHandle(string key) {
            Key = key;
        }

        // InstantiateAsync
        public Task<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform? parent, MonoBehaviour owner, Action<PrefabHandle>? onDestroy = null) {
            return InstantiateAsync( position, rotation, parent, owner.destroyCancellationToken, onDestroy );
        }
        public Task<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform? parent, CancellationToken destroyCancellationToken, Action<PrefabHandle>? onDestroy = null) {
            Assert.Operation.Message( $"PrefabHandle {this} must be non-active" ).Valid( instance == null );
            instance = Addressables.InstantiateAsync( Key, new InstantiationParameters( position, rotation, parent ) );
            var disposable = default( CancellationTokenRegistration );
            disposable = destroyCancellationToken.Register( () => {
                if (IsActive) {
                    onDestroy?.Invoke( this );
                    if (IsActive) Destroy();
                }
                disposable.Dispose();
            } );
            return GetInstanceAsync( destroyCancellationToken );
        }
        public async Task<GameObject> GetInstanceAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be active" ).Valid( instance != null );
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.Value.IsValid() );
            if (instance.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await instance.Value.Task.WaitAsync( cancellationToken ).ConfigureAwait( false );
                if (instance.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw instance.Value.OperationException;
        }
        public void Destroy() {
            Assert.Operation.Message( $"PrefabHandle {this} must be active" ).Valid( instance != null );
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.Value.IsValid() );
            Addressables.ReleaseInstance( instance.Value );
            instance = null;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class PrefabHandleExtensions {

        public static void DestroyAll<T>(this IEnumerable<PrefabHandle> collection) {
            foreach (var item in collection.Where( i => i.IsActive )) {
                item.Destroy();
            }
        }

    }
}

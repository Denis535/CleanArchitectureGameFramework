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
        public Task<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be non-active" ).Valid( instance == null );
            instance = Addressables.InstantiateAsync( key, new InstantiationParameters( position, rotation, parent ) );
            return GetInstanceAsync( cancellationToken );
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
        public void DestroyImmediate() {
            Assert.Operation.Message( $"PrefabHandle {this} must be active" ).Valid( instance != null );
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( instance.Value.IsValid() );
            UnityEngine.Object.DestroyImmediate( instance.Value.Result );
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
        public static void DestroyImmediateAll<T>(this IEnumerable<PrefabHandle> collection) {
            foreach (var item in collection.Where( i => i.IsActive )) {
                item.DestroyImmediate();
            }
        }

    }
}
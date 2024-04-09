#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceLocations;

    public abstract class PrefabHandleBase<T> where T : notnull, Component {

        protected AsyncOperationHandle<T> PrefabHandle { get; private set; }
        public bool IsValid => PrefabHandle.IsValid();
        public bool IsSucceeded => PrefabHandle.IsValid() && PrefabHandle.IsSucceeded();
        public bool IsFailed => PrefabHandle.IsValid() && PrefabHandle.IsFailed();
        public T Prefab {
            get {
                Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
                Assert.Operation.Message( $"PrefabHandle {this} must be succeeded" ).Valid( PrefabHandle.IsSucceeded() );
                return PrefabHandle.Result;
            }
        }
        private List<T> Instances_ { get; } = new List<T>( 1 );
        public IReadOnlyList<T> Instances {
            get {
                Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
                return Instances_;
            }
        }

        // Constructor
        public PrefabHandleBase() {
        }

        // LoadPrefabAsync
        protected Task<T> LoadPrefabAsync(IResourceLocation location, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !PrefabHandle.IsValid() );
            PrefabHandle = LoadPrefabAsync( location );
            return GetPrefabAsync( cancellationToken );
        }
        protected Task<T> LoadPrefabAsync(AsyncOperationHandle<T> prefabHandle, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !PrefabHandle.IsValid() );
            PrefabHandle = prefabHandle;
            return GetPrefabAsync( cancellationToken );
        }
        public async Task<T> GetPrefabAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            if (PrefabHandle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await PrefabHandle.Task.WaitAsync( cancellationToken );
                if (PrefabHandle.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw PrefabHandle.OperationException;
        }

        // Release
        public void Release() {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            Assert.Operation.Message( $"PrefabHandle {this} must have no instances, but have {Instances_.Count} instances" ).Valid( !Instances_.Any() );
            Addressables.Release( PrefabHandle );
            PrefabHandle = default;
        }
        public void ReleaseSafe() {
            if (PrefabHandle.IsValid()) {
                Release();
            }
        }

        // InstantiateAsync
        public async Task<T> InstantiateAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            var prefab = await GetPrefabAsync( cancellationToken );
            var instance = UnityEngine.Object.Instantiate( prefab );
            Instances_.Add( instance );
            return instance;
        }
        public async Task<T> InstantiateAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            var prefab = await GetPrefabAsync( cancellationToken );
            var instance = UnityEngine.Object.Instantiate( prefab, position, rotation );
            Instances_.Add( instance );
            return instance;
        }
        public async Task<T> InstantiateAsync(Transform parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            var prefab = await GetPrefabAsync( cancellationToken );
            var instance = UnityEngine.Object.Instantiate( prefab, parent );
            Instances_.Add( instance );
            return instance;
        }
        public async Task<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            var prefab = await GetPrefabAsync( cancellationToken );
            var instance = UnityEngine.Object.Instantiate( prefab, position, rotation, parent );
            Instances_.Add( instance );
            return instance;
        }

        // Destroy
        public void Destroy(T instance) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            Assert.Operation.Message( $"PrefabHandle {this} must have {instance} instance" ).Valid( Instances_.Contains( instance ) );
            UnityEngine.Object.Destroy( instance.gameObject );
            Instances_.Remove( instance );
        }
        public void DestroyImmediate(T instance) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            Assert.Operation.Message( $"PrefabHandle {this} must have {instance} instance" ).Valid( Instances_.Contains( instance ) );
            UnityEngine.Object.DestroyImmediate( instance.gameObject );
            Instances_.Remove( instance );
        }

        // Destroy
        public void Destroy(IEnumerable<T> instances) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            foreach (var instance in instances) {
                Destroy( instance );
            }
        }
        public void DestroyImmediate(IEnumerable<T> instances) {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            foreach (var instance in instances) {
                DestroyImmediate( instance );
            }
        }

        // Destroy
        public void DestroyAll() {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            while (Instances_.Any()) {
                Destroy( Instances_.Last() );
            }
        }
        public void DestroyAllImmediate() {
            Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
            while (Instances_.Any()) {
                DestroyImmediate( Instances_.Last() );
            }
        }

        // Utils
        public override string ToString() {
            return PrefabHandle.DebugName;
        }

        // Helpers
        protected static AsyncOperationHandle<T> LoadPrefabAsync(string key) {
            var prefab = Addressables.LoadAssetAsync<GameObject>( key );
            return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( prefab, i => {
                var result = (T?) i.Result.GetComponent<T>();
                if (result != null) {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( result, null );
                } else {
                    return Addressables.ResourceManager.CreateCompletedOperation<T>( null!, $"Component '{typeof( T )}' was not found" );
                }
            } );
        }
        protected static AsyncOperationHandle<T> LoadPrefabAsync(IResourceLocation location) {
            var prefab = Addressables.LoadAssetAsync<GameObject>( location );
            return Addressables.ResourceManager.CreateChainOperation<T, GameObject>( prefab, i => {
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

        // LoadPrefabAsync
        public Task<T> LoadPrefabAsync(string key, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !PrefabHandle.IsValid() );
            return LoadPrefabAsync( LoadPrefabAsync( key ), cancellationToken );
        }

    }
    // DynamicPrefabHandle
    public class DynamicPrefabHandle<T> : PrefabHandleBase<T> where T : notnull, Component {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert.Operation.Message( $"PrefabHandle {this} must be valid" ).Valid( PrefabHandle.IsValid() );
                return key!;
            }
            private set {
                key = value;
            }
        }

        // Constructor
        public DynamicPrefabHandle() {
        }

        // LoadPrefabAsync
        public Task<T> LoadPrefabAsync(string key, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"PrefabHandle {this} is already valid" ).Valid( !PrefabHandle.IsValid() );
            return LoadPrefabAsync( LoadPrefabAsync( key ), cancellationToken );
        }

    }
}

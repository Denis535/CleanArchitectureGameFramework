#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public abstract class AddressableHandle<T> {

        protected internal AsyncOperationHandle<T> Handle { get; set; }
        public bool IsValid => Handle.IsValid();
        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        public T Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public T? ValueSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }
        public Exception? Exception => Handle.OperationException;

        // Constructor
        public AddressableHandle() {
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

        // Heleprs
        protected internal void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandle {this} must be valid" ).Valid( Handle.IsValid() );
        }
        protected internal void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableHandle {this} must be succeeded" ).Valid( Handle.IsSucceeded() );
        }
        protected internal void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandle {this} is already valid" ).Valid( !Handle.IsValid() );
        }

    }
    // AddressableAssetHandle
    public abstract class AddressableAssetHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressableAssetHandle() {
        }

        // GetValueAsync
        public async Task<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

        // Release
        public void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
        public void ReleaseSafe() {
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    // AddressablePrefabHandle
    public abstract class AddressablePrefabHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressablePrefabHandle() {
        }

        // GetValueAsync
        public async Task<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

        // Release
        public void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
        public void ReleaseSafe() {
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    // AddressableSceneHandle
    public abstract class AddressableSceneHandle : AddressableHandle<SceneInstance> {

        // Constructor
        public AddressableSceneHandle() {
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            await value.ActivateAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return value.Scene;
        }

        // GetValueAsync
        public async Task<Scene> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value.Scene;
        }

        // UnloadAsync
        public async Task UnloadAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).Task.WaitAsync( cancellationToken );
            Handle = default;
        }
        public async Task UnloadSafeAsync(CancellationToken cancellationToken) {
            if (Handle.IsValid()) {
                await UnloadAsync( cancellationToken );
            }
        }

    }
    // AddressableInstanceHandle
    public class AddressableInstanceHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressableInstanceHandle() {
        }

        // GetValueAsync
        public async Task<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

        // ReleaseInstance
        public void ReleaseInstance() {
            Assert_IsValid();
            Addressables.ReleaseInstance( Handle );
            Handle = default;
        }
        public void ReleaseInstanceSafe() {
            if (Handle.IsValid()) {
                ReleaseInstance();
            }
        }

    }
}

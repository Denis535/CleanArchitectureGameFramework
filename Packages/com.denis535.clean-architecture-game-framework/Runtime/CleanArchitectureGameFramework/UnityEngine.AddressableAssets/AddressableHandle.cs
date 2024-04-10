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

        protected AsyncOperationHandle<T> Handle { get; set; }
        public bool IsValid => Handle.IsValid();
        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        public T Result {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }

        // Constructor
        public AddressableHandle() {
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

        // Heleprs
        protected void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandle {this} must be valid" ).Valid( Handle.IsValid() );
        }
        protected void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableHandle {this} must be succeeded" ).Valid( Handle.IsSucceeded() );
        }
        protected void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandle {this} is already valid" ).Valid( !Handle.IsValid() );
        }

    }
    // AddressableAssetHandle
    public abstract class AddressableAssetHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressableAssetHandle() {
        }

        // GetResultAsync
        public Task<T> GetResultAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
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

        // GetResultAsync
        public Task<T> GetResultAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
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
            var result = await Handle.GetResultAsync( cancellationToken );
            await result.ActivateAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return result.Scene;
        }

        // GetResultAsync
        public async Task<Scene> GetResultAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
        }

        // UnloadAsync
        public async Task UnloadAsync() {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).Task;
            Handle = default;
        }
        public async Task UnloadSafeAsync() {
            if (Handle.IsValid()) {
                await UnloadAsync();
            }
        }

    }
    // AddressableInstanceHandle
    public class AddressableInstanceHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressableInstanceHandle() {
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

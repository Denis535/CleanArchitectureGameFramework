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

    public class SceneHandle : AddressableHandle<SceneInstance> {

        public new Scene Value => base.Value.Scene;

        // Constructor
        public SceneHandle(string key) : base( key ) {
        }
        public SceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key, handle ) {
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( cancellationToken );
            return value.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            await value.ActivateAsync();
            cancellationToken.ThrowIfCancellationRequested();
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
    public class DynamicSceneHandle : DynamicAddressableHandle<SceneInstance> {

        public new Scene Value => base.Value.Scene;

        // Constructor
        public DynamicSceneHandle() {
        }
        public DynamicSceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key, handle ) {
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(string key, LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( cancellationToken );
            return value.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            await value.ActivateAsync();
            cancellationToken.ThrowIfCancellationRequested();
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
}

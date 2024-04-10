#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneHandle : AddressableHandle<SceneInstance, string> {

        public new Scene Result => base.Result.Scene;

        // Constructor
        public SceneHandle(string key) : base( key ) {
        }

        // LoadSceneAsync
        public async Task<Scene> LoadSceneAsync(LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
        }

        // GetResultAsync
        public async Task<Scene> GetResultAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync() {
            Assert_IsValid();
            var result = Handle.Result;
            await result.ActivateAsync();
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
    public class DynamicSceneHandle : DynamicAddressableHandle<SceneInstance, string> {

        public new Scene Result => base.Result.Scene;

        // Constructor
        public DynamicSceneHandle() {
        }

        // LoadSceneAsync
        public async Task<Scene> LoadSceneAsync(string key, LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad );
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
        }

        // GetResultAsync
        public async Task<Scene> GetResultAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync() {
            Assert_IsValid();
            var result = Handle.Result;
            await result.ActivateAsync();
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
}

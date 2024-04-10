#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneHandle : AddressableHandle<SceneInstance> {

        public string Key { get; }
        public new Scene Result => base.Result.Scene;

        // Constructor
        public SceneHandle(string key) {
            Key = key;
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
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
    public class DynamicSceneHandle : AddressableHandle<SceneInstance> {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }
        public new Scene Result => base.Result.Scene;

        // Constructor
        public DynamicSceneHandle() {
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(string key, LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad );
            var result = await Handle.GetResultAsync( cancellationToken );
            return result.Scene;
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
}

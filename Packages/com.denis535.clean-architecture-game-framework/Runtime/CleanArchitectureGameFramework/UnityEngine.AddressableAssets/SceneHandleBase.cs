#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public abstract class SceneHandleBase {

        protected AsyncOperationHandle<SceneInstance> scene;

        public bool IsValid => scene.IsValid();
        public bool IsSucceeded => scene.IsValid() && scene.IsSucceeded();
        public bool IsFailed => scene.IsValid() && scene.IsFailed();
        public Scene Scene {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.IsValid() );
                Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( scene.IsSucceeded() );
                return scene.Result.Scene;
            }
        }

        // Constructor
        public SceneHandleBase() {
        }

        // LoadSceneAsync
        protected Task<Scene> LoadSceneAsync(AsyncOperationHandle<SceneInstance> scene, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !this.scene.IsValid() );
            this.scene = scene;
            return GetSceneAsync( cancellationToken );
        }
        public async Task<Scene> GetSceneAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.IsValid() );
            if (scene.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await scene.Task.WaitAsync( cancellationToken );
                if (scene.Status is AsyncOperationStatus.Succeeded) {
                    return result.Scene;
                }
            }
            throw scene.OperationException;
        }
        public async Task<Scene> ActivateAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.IsValid() );
            var sceneInstance = scene.Result;
            await sceneInstance.ActivateAsync();
            return sceneInstance.Scene;
        }
        public async Task UnloadAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.IsValid() );
            await Addressables.UnloadSceneAsync( scene ).Task;
            scene = default;
        }
        public async Task UnloadSafeAsync() {
            if (scene.IsValid()) {
                await UnloadAsync();
            }
        }

        // Utils
        public override string ToString() {
            return scene.DebugName;
        }

    }
    // SceneHandle
    public class SceneHandle : SceneHandleBase {

        public string Key { get; }

        // Constructor
        public SceneHandle(string key) {
            Key = key;
        }

        // LoadSceneAsync
        public Task<Scene> LoadSceneAsync(LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !scene.IsValid() );
            return LoadSceneAsync( Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad ), cancellationToken );
        }

    }
    // DynamicSceneHandle
    public class DynamicSceneHandle : SceneHandleBase {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.IsValid() );
                return key!;
            }
            private set {
                key = value;
            }
        }

        // Constructor
        public DynamicSceneHandle() {
        }

        // LoadSceneAsync
        public Task<Scene> LoadSceneAsync(string key, LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !scene.IsValid() );
            return LoadSceneAsync( Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad ), cancellationToken );
        }

    }
}

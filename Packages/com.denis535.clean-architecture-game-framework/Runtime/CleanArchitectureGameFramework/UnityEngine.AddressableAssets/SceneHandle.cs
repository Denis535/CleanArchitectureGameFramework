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

    public class SceneHandle {

        private AsyncOperationHandle<SceneInstance> scene;

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
        public SceneHandle() {
        }

        // LoadSceneAsync
        public Task<Scene> LoadSceneAsync(string key, LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !scene.IsValid() );
            scene = Addressables.LoadSceneAsync( key, loadMode, activateOnLoad );
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
            await Addressables.UnloadSceneAsync( scene ).Task;
            scene = default;
        }

        // Utils
        public override string ToString() {
            return scene.DebugName;
        }

    }
}

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
    using UnityEngine.SceneManagement;

    public class SceneHandle {

        private AsyncOperationHandle<SceneInstance>? scene;

        public string Key { get; }
        public bool IsActive => scene != null;
        public bool IsValid => scene != null && scene.Value.IsValid();
        public bool IsSucceeded => scene != null && scene.Value.IsValid() && scene.Value.IsSucceeded();
        public bool IsFailed => scene != null && scene.Value.IsValid() && scene.Value.IsFailed();
        public Scene Scene {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( scene != null );
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.Value.IsValid() );
                Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( scene.Value.IsSucceeded() );
                return scene.Value.Result.Scene;
            }
        }

        // Constructor
        public SceneHandle(string key) {
            Key = key;
        }

        // LoadSceneAsync
        public Task<Scene> LoadSceneAsync(LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} must be non-active" ).Valid( scene == null );
            scene = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            return GetSceneAsync( cancellationToken );
        }
        public async Task<Scene> GetSceneAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( scene != null );
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.Value.IsValid() );
            if (scene.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await scene.Value.Task.WaitAsync( cancellationToken ).ConfigureAwait( false );
                if (scene.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result.Scene;
                }
            }
            throw scene.Value.OperationException;
        }
        public async Task<Scene> ActivateAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( scene != null );
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.Value.IsValid() );
            var sceneInstance = scene.Value.Result;
            await sceneInstance.ActivateAsync();
            return sceneInstance.Scene;
        }
        public async Task UnloadAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( scene != null );
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.Value.IsValid() );
            await Addressables.UnloadSceneAsync( scene.Value ).Task.ConfigureAwait( false );
            scene = null;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class SceneHandleExtensions {

        public static async Task UnloadAllAsync(this IEnumerable<SceneHandle> collection) {
            foreach (var item in collection.Where( i => i.IsActive )) {
                await item.UnloadAsync();
            }
        }

    }
}

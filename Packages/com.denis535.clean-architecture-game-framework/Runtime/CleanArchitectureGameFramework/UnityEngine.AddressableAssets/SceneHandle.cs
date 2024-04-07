#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneHandle {

        private AsyncOperationHandle<SceneInstance> scene;

        public string Key { get; }
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
        public SceneHandle(string key) {
            Key = key;
        }

        // LoadSceneAsync
        public Task<Scene> LoadSceneAsync(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert.Operation.Message( $"SceneHandle {this} already exists" ).Valid( !scene.IsValid() );
            scene = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            return GetSceneAsync();
        }
        public async Task<Scene> GetSceneAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( scene.IsValid() );
            if (scene.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await scene.Task.ConfigureAwait( false );
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
            await Addressables.UnloadSceneAsync( scene ).Task.ConfigureAwait( false );
            scene = default;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class SceneHandleExtensions {

        public static async Task UnloadAllAsync(this IEnumerable<SceneHandle> collection) {
            foreach (var item in collection.Where( i => i.IsValid )) {
                await item.UnloadAsync();
            }
        }

    }
}

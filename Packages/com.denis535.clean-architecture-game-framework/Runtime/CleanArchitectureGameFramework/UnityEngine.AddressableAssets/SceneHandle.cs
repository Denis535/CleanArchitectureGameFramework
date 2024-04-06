#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class SceneHandle {

        private AsyncOperationHandle<SceneInstance>? handle;

        public string Key { get; }
        public bool IsActive => handle != null;
        public bool IsValid => handle != null && handle.Value.IsValid();
        public bool IsSucceeded => handle != null && handle.Value.IsValid() && handle.Value.IsSucceeded();
        public bool IsFailed => handle != null && handle.Value.IsValid() && handle.Value.IsFailed();
        public Scene Scene {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( handle != null );
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( handle.Value.IsValid() );
                Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( handle.Value.IsSucceeded() );
                return handle.Value.Result.Scene;
            }
        }

        // Constructor
        public SceneHandle(string key) {
            Key = key;
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert.Operation.Message( $"SceneHandle {this} must be non-active" ).Valid( handle == null );
            handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            if (handle.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await handle.Value.Task.ConfigureAwait( false );
                if (handle.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result.Scene;
                }
            }
            throw handle.Value.OperationException;
        }
        public async Task<Scene> ActivateAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( handle != null );
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( handle.Value.IsValid() );
            Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( handle.Value.IsSucceeded() );
            var sceneInstance = handle.Value.Result;
            await sceneInstance.ActivateAsync();
            return sceneInstance.Scene;
        }
        public async Task UnloadAsync() {
            if (handle != null) {
                await Addressables.UnloadSceneAsync( handle.Value ).Task.ConfigureAwait( false );
                handle = null;
            }
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public static class SceneHandleExtensions {

        public static async Task UnloadAsync(this IEnumerable<SceneHandle> collection) {
            foreach (var item in collection) {
                await item.UnloadAsync();
            }
        }

    }
}

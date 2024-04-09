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
    using UnityEngine.ResourceManagement.ResourceLocations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public abstract class SceneHandleBase {

        protected AsyncOperationHandle<SceneInstance> SceneHandle { get; private set; }
        public bool IsValid => SceneHandle.IsValid();
        public bool IsSucceeded => SceneHandle.IsValid() && SceneHandle.IsSucceeded();
        public bool IsFailed => SceneHandle.IsValid() && SceneHandle.IsFailed();
        public Scene Scene {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( SceneHandle.IsValid() );
                Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( SceneHandle.IsSucceeded() );
                return SceneHandle.Result.Scene;
            }
        }

        // Constructor
        public SceneHandleBase() {
        }

        // LoadSceneAsync
        protected Task<Scene> LoadSceneAsync(IResourceLocation location, LoadSceneMode mode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !SceneHandle.IsValid() );
            SceneHandle = Addressables.LoadSceneAsync( location, mode, activateOnLoad );
            return GetSceneAsync( cancellationToken );
        }
        protected Task<Scene> LoadSceneAsync(AsyncOperationHandle<SceneInstance> sceneHandle, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !SceneHandle.IsValid() );
            SceneHandle = sceneHandle;
            return GetSceneAsync( cancellationToken );
        }
        public async Task<Scene> GetSceneAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( SceneHandle.IsValid() );
            if (SceneHandle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await SceneHandle.Task.WaitAsync( cancellationToken );
                if (SceneHandle.Status is AsyncOperationStatus.Succeeded) {
                    return result.Scene;
                }
            }
            throw SceneHandle.OperationException;
        }
        public async Task<Scene> ActivateAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( SceneHandle.IsValid() );
            var sceneInstance = SceneHandle.Result;
            await sceneInstance.ActivateAsync();
            return sceneInstance.Scene;
        }

        // UnloadAsync
        public async Task UnloadAsync() {
            Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( SceneHandle.IsValid() );
            await Addressables.UnloadSceneAsync( SceneHandle ).Task;
            SceneHandle = default;
        }
        public async Task UnloadSafeAsync() {
            if (SceneHandle.IsValid()) {
                await UnloadAsync();
            }
        }

        // Utils
        public override string ToString() {
            return SceneHandle.DebugName;
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
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !SceneHandle.IsValid() );
            return LoadSceneAsync( Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad ), cancellationToken );
        }

    }
    // DynamicSceneHandle
    public class DynamicSceneHandle : SceneHandleBase {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( SceneHandle.IsValid() );
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
            Assert.Operation.Message( $"SceneHandle {this} is already valid" ).Valid( !SceneHandle.IsValid() );
            return LoadSceneAsync( Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad ), cancellationToken );
        }

    }
}

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

        protected AsyncOperationHandle<SceneInstance> SceneHandle { get; private set; }
        public bool IsValid => SceneHandle.IsValid();
        public bool IsSucceeded => SceneHandle.IsValid() && SceneHandle.IsSucceeded();
        public bool IsFailed => SceneHandle.IsValid() && SceneHandle.IsFailed();
        public Scene Scene {
            get {
                this.Assert_IsValid();
                this.Assert_IsSucceeded();
                return SceneHandle.Result.Scene;
            }
        }

        // Constructor
        public SceneHandleBase() {
        }

        // Initialize
        //protected void Initialize(IResourceLocation location, LoadSceneMode loadMode, bool activateOnLoad) {
        //    this.Assert_IsNotValid();
        //    SceneHandle = Addressables.LoadSceneAsync( location, loadMode, activateOnLoad );
        //}
        protected void Initialize(AsyncOperationHandle<SceneInstance> handle) {
            this.Assert_IsNotValid();
            SceneHandle = handle;
        }

        // GetSceneAsync
        public async Task<Scene> GetSceneAsync(CancellationToken cancellationToken) {
            this.Assert_IsValid();
            var result = await SceneHandle.GetResultAsync( cancellationToken );
            return result.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync() {
            this.Assert_IsValid();
            var sceneInstance = SceneHandle.Result;
            await sceneInstance.ActivateAsync();
            return sceneInstance.Scene;
        }

        // UnloadAsync
        public async Task UnloadAsync() {
            this.Assert_IsValid();
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
            this.Assert_IsNotValid();
            Initialize( Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad ) );
            return GetSceneAsync( cancellationToken );
        }

    }
    // DynamicSceneHandle
    public class DynamicSceneHandle : SceneHandleBase {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                this.Assert_IsValid();
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
            this.Assert_IsNotValid();
            Initialize( Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad ) );
            return GetSceneAsync( cancellationToken );
        }

    }
}

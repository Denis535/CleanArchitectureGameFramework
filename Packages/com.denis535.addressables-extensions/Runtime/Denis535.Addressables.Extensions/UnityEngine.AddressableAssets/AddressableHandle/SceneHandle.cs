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

    public class SceneHandle : AddressableHandle, ISceneHandle<SceneHandle> {

        // Key
        public override string Key { get; }
        // Handle
        private AsyncOperationHandle<SceneInstance> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsValid() && Handle.IsDone;
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();

        // Constructor
        public SceneHandle(string key) {
            Key = key;
        }
        public SceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) {
            Key = key;
            Handle = handle;
        }

        // Load
        public SceneHandle Load(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            return this;
        }

        // WaitAsync
        public ValueTask WaitAsync() {
            Assert_IsValid();
            return Handle.WaitAsync( default );
        }

        // GetValueAsync
        public async ValueTask<Scene> GetValueAsync() {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( default );
            return value.Scene;
        }

        // ActivateAsync
        public async ValueTask<Scene> ActivateAsync() {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( default );
            await value.ActivateAsync();
            return value.Scene;
        }

        // Unload
        public void Unload() {
            Assert_IsValid();
            Addressables.UnloadSceneAsync( Handle ).Wait();
            Handle = default;
        }
        public async ValueTask UnloadAsync() {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).WaitAsync( default );
            Handle = default;
        }

        // UnloadSafe
        public void UnloadSafe() {
            if (Handle.IsValid()) {
                Unload();
            }
        }
        public async ValueTask UnloadSafeAsync() {
            if (Handle.IsValid()) {
                await UnloadAsync();
            }
        }

        // Utils
        public override string ToString() {
            return "SceneHandle: " + Key;
        }

    }
}

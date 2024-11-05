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

    public class SceneHandle : AddressableHandle<SceneInstance> {

        // Constructor
        public SceneHandle(string key) : base( key ) {
        }

        // Load
        public SceneHandle Load(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            return this;
        }

        // Wait
        public ValueTask WaitAsync() {
            Assert_IsValid();
            return Handle.WaitAsync( default );
        }

        // GetValue
        public async ValueTask<Scene> GetValueAsync() {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( default );
            return value.Scene;
        }

        // Activate
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

        // Unload
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

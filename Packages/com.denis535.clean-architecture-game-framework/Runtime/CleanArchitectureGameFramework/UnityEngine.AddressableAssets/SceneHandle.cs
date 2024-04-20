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

    public class SceneHandle : AddressableHandleBase3<SceneInstance> {

        // Value
        public new Scene Value => base.Value.Scene;

        // Constructor
        public SceneHandle(string key) : base( key ) {
        }
        public SceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key, handle ) {
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( default );
            return value.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync() {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( default );
            await value.ActivateAsync();
            return value.Scene;
        }

        // UnloadAsync
        public async Task UnloadAsync() {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).Task.WaitAsync( default );
            Handle = default;
        }
        public async Task UnloadSafeAsync() {
            if (Handle.IsValid()) {
                await UnloadAsync();
            }
        }

    }
    public class DynamicSceneHandle : DynamicAddressableHandleBase3<SceneInstance> {

        // Value
        public new Scene Value => base.Value.Scene;

        // Constructor
        public DynamicSceneHandle() {
        }
        public DynamicSceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key, handle ) {
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(string key, LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( default );
            return value.Scene;
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync() {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( default );
            await value.ActivateAsync();
            return value.Scene;
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

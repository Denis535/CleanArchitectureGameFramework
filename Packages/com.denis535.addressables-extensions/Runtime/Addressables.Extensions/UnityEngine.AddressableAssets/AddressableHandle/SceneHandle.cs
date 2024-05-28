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

    public class SceneHandle : AddressableHandle {

        // Handle
        private AsyncOperationHandle<SceneInstance> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsFailed();
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public SceneHandle(string key) : base( key ) {
        }
        public SceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key ) {
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
        public async ValueTask ActivateAsync() {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( default );
            await value.ActivateAsync();
        }

        // UnloadAsync
        public async ValueTask UnloadAsync() {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).Task.WaitAsync( default );
            Handle = default;
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

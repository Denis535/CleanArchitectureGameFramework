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

    public class SceneHandle : AddressableSceneHandle {

        public string Key { get; }
        public new Scene Value => base.Value.Scene;

        // Constructor
        public SceneHandle(string key) {
            Key = key;
        }
        public SceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) {
            Key = key;
            Handle = handle;
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( cancellationToken );
            return value.Scene;
        }

    }
    public class DynamicSceneHandle : AddressableSceneHandle {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }
        public new Scene Value => base.Value.Scene;

        // Constructor
        public DynamicSceneHandle() {
        }
        public DynamicSceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) {
            Key = key;
            Handle = handle;
        }

        // LoadAsync
        public async Task<Scene> LoadAsync(string key, LoadSceneMode loadMode, bool activateOnLoad, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( cancellationToken );
            return value.Scene;
        }

    }
}

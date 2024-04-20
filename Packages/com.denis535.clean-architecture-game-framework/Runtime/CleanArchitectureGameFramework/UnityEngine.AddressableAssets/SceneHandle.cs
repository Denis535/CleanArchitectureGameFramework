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

        // Key
        public new string Key => base.Key!;
        // Handle
        protected AsyncOperationHandle<SceneInstance> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public Scene Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result.Scene;
            }
        }
        public Scene? ValueSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result.Scene : default;
            }
        }

        // Constructor
        public SceneHandle(string key) : base( key ) {
        }
        public SceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key ) {
            Handle = handle;
        }

        // LoadAsync
        public async ValueTask<Scene> LoadAsync(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( default );
            return value.Scene;
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

    }
    public class DynamicSceneHandle : AddressableHandle {

        // Handle
        protected AsyncOperationHandle<SceneInstance> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public Scene Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result.Scene;
            }
        }
        public Scene? ValueSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result.Scene : default;
            }
        }

        // Constructor
        public DynamicSceneHandle() : base( null ) {
        }
        public DynamicSceneHandle(string key, AsyncOperationHandle<SceneInstance> handle) : base( key ) {
            Handle = handle;
        }

        // LoadAsync
        public async ValueTask<Scene> LoadAsync(string key, LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsNotValid();
            Handle = Addressables.LoadSceneAsync( Key = key, loadMode, activateOnLoad );
            var value = await Handle.GetResultAsync( default );
            return value.Scene;
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

        // UnloadAsync
        public async ValueTask UnloadAsync() {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).Task;
            Key = null;
            Handle = default;
        }
        public async ValueTask UnloadSafeAsync() {
            if (Handle.IsValid()) {
                await UnloadAsync();
            }
        }

    }
}

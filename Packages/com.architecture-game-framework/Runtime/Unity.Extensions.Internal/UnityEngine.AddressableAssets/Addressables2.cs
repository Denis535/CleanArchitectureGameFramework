#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;
    using Object = Object;

    public static partial class Addressables2 {

        // LoadAsset/Async
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string key) where T : Object {
            return Addressables.LoadAssetAsync<T>( key );
        }
        public static AsyncOperationHandle<IList<T>> LoadAssetsAsync<T>(params string[] keys) where T : Object {
            return Addressables.LoadAssetsAsync<T>( (IEnumerable) keys, null!, Addressables.MergeMode.Union );
        }
        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(string key, LoadSceneMode mode, bool activateOnLoad) {
            return Addressables.LoadSceneAsync( key, mode, activateOnLoad );
        }

        // Release
        public static void Release<T>(AsyncOperationHandle<T> handle) where T : Object {
            Addressables.Release( handle );
        }
        public static void Release<T>(AsyncOperationHandle<IList<T>> handle) where T : Object {
            Addressables.Release( handle );
        }
        public static bool ReleaseInstance(AsyncOperationHandle<GameObject> handle) {
            return Addressables.ReleaseInstance( handle );
        }
        public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle) {
            return Addressables.UnloadSceneAsync( handle );
        }

        // Release
        public static void Release<T>(T @object) where T : Object {
            Addressables.Release( @object );
        }
        public static void Release<T>(IList<T> objects) where T : Object {
            Addressables.Release( objects );
        }
        public static AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance instance) {
            return Addressables.UnloadSceneAsync( instance );
        }

    }
    public static partial class Addressables2 {

        // Instantiate/Async
        public static AsyncOperationHandle<GameObject> InstantiateAsync(string key) {
            return Addressables.InstantiateAsync( key, new InstantiationParameters() );
        }
        public static AsyncOperationHandle<GameObject> InstantiateAsync(string key, Transform parent, bool instantiateInWorldSpace) {
            return Addressables.InstantiateAsync( key, new InstantiationParameters( parent, instantiateInWorldSpace ) );
        }
        public static AsyncOperationHandle<GameObject> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform? parent = null) {
            return Addressables.InstantiateAsync( key, new InstantiationParameters( position, rotation, parent ) );
        }

        // Release
        public static bool ReleaseInstance(GameObject gameObject) {
            return Addressables.ReleaseInstance( gameObject );
        }

    }
}

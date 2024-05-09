#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public static class Addressables2 {

        // Instantiate
        public static GameObject Instantiate(string key) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult();
                var instance = UnityEngine.Object.Instantiate( prefab );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static GameObject Instantiate(string key, Transform? parent) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult();
                var instance = UnityEngine.Object.Instantiate( prefab, parent );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static GameObject Instantiate(string key, Transform? parent, bool instantiateInWorldSpace) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult();
                var instance = UnityEngine.Object.Instantiate( prefab, parent, instantiateInWorldSpace );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static GameObject Instantiate(string key, Vector3 position, Quaternion rotation) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult();
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static GameObject Instantiate(string key, Vector3 position, Quaternion rotation, Transform? parent) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult();
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation, parent );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static GameObject Instantiate(string key, Func<GameObject, GameObject> instanceProvider) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult();
                var instance = instanceProvider( prefab );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }

        // InstantiateAsync
        public static async ValueTask<GameObject> InstantiateAsync(string key, CancellationToken cancellationToken) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = await prefabHandle.GetResultAsync( cancellationToken );
                var instance = UnityEngine.Object.Instantiate( prefab );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<GameObject> InstantiateAsync(string key, Transform? parent, CancellationToken cancellationToken) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = await prefabHandle.GetResultAsync( cancellationToken );
                var instance = UnityEngine.Object.Instantiate( prefab, parent );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<GameObject> InstantiateAsync(string key, Transform? parent, bool instantiateInWorldSpace, CancellationToken cancellationToken) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = await prefabHandle.GetResultAsync( cancellationToken );
                var instance = UnityEngine.Object.Instantiate( prefab, parent, instantiateInWorldSpace );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<GameObject> InstantiateAsync(string key, Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = await prefabHandle.GetResultAsync( cancellationToken );
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<GameObject> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = await prefabHandle.GetResultAsync( cancellationToken );
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation, parent );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<GameObject> InstantiateAsync(string key, Func<GameObject, GameObject> instanceProvider, CancellationToken cancellationToken) {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = await prefabHandle.GetResultAsync( cancellationToken );
                var instance = instanceProvider( prefab );
                instance.AddDestroyable( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }

    }
}

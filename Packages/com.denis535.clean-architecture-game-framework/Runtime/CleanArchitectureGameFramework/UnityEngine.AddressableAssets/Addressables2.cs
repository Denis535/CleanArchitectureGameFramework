#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class Addressables2 {

        // Instantiate
        public static T Instantiate<T>(string key) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult().RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static T Instantiate<T>(string key, Transform? parent) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult().RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab, parent );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static T Instantiate<T>(string key, Vector3 position, Quaternion rotation) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult().RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static T Instantiate<T>(string key, Vector3 position, Quaternion rotation, Transform? parent) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult().RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation, parent );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static T Instantiate<T>(string key, Func<T, T> instanceProvider) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = prefabHandle.GetResult().RequireComponent<T>();
                var instance = instanceProvider( prefab );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }

        // InstantiateAsync
        public static async ValueTask<T> InstantiateAsync<T>(string key, CancellationToken cancellationToken) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = (await prefabHandle.GetResultAsync( cancellationToken )).RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<T> InstantiateAsync<T>(string key, Transform? parent, CancellationToken cancellationToken) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = (await prefabHandle.GetResultAsync( cancellationToken )).RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab, parent );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation, CancellationToken cancellationToken) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = (await prefabHandle.GetResultAsync( cancellationToken )).RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<T> InstantiateAsync<T>(string key, Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = (await prefabHandle.GetResultAsync( cancellationToken )).RequireComponent<T>();
                var instance = UnityEngine.Object.Instantiate( prefab, position, rotation, parent );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }
        public static async ValueTask<T> InstantiateAsync<T>(string key, Func<T, T> instanceProvider, CancellationToken cancellationToken) where T : notnull, Component {
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>( key );
            try {
                var prefab = (await prefabHandle.GetResultAsync( cancellationToken )).RequireComponent<T>();
                var instance = instanceProvider( prefab );
                instance.gameObject.AddAddressableInstance( prefabHandle );
                return instance;
            } catch {
                Addressables.Release( prefabHandle );
                throw;
            }
        }

    }
}

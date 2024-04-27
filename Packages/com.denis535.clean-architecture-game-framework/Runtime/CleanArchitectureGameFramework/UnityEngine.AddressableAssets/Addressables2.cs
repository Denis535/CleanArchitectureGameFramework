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

        // IsState
        public static bool IsNone<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.None;
        }
        public static bool IsSucceeded<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.Succeeded;
        }
        public static bool IsFailed<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.Failed;
        }

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

        // Wait
        public static void Wait<T>(this AsyncOperationHandle<T> handle) {
            if (!handle.IsFailed()) {
                handle.WaitForCompletion();
                if (handle.IsSucceeded()) {
                    return;
                }
            }
            throw handle.OperationException;
        }
        public static async ValueTask WaitAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (!handle.IsFailed()) {
                await handle.Task.WaitAsync( cancellationToken );
                cancellationToken.ThrowIfCancellationRequested();
                if (handle.IsSucceeded()) {
                    return;
                }
            }
            throw handle.OperationException;
        }

        // GetResult
        public static T GetResult<T>(this AsyncOperationHandle<T> handle) {
            if (!handle.IsFailed()) {
                var result = handle.WaitForCompletion();
                if (handle.IsSucceeded()) {
                    return result;
                }
            }
            throw handle.OperationException;
        }
        public static async ValueTask<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            if (!handle.IsFailed()) {
                var result = await handle.Task.WaitAsync( cancellationToken );
                cancellationToken.ThrowIfCancellationRequested();
                if (handle.IsSucceeded()) {
                    return result;
                }
            }
            throw handle.OperationException;
        }

    }
}

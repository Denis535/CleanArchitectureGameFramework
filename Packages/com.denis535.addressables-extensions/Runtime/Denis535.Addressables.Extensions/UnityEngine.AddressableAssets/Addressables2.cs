#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    internal static class Addressables2 {

        // LoadAssetAsync
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object {
            return Addressables.LoadAssetAsync<T>( key );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadAssetListAsync<T>(string[] keys) where T : UnityEngine.Object {
            return Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).Chain( dependency => {
                var result = (IReadOnlyList<T>) dependency.Result;
                return Addressables.ResourceManager.CreateCompletedOperation( result, null );
            } );
        }

        // LoadPrefabAsync
        public static AsyncOperationHandle<T> LoadPrefabAsync<T>(string key) where T : UnityEngine.Object {
            return Addressables.LoadAssetAsync<GameObject>( key ).Chain( dependency => {
                var result = dependency.Result.GetComponent<T?>() ?? throw new InvalidOperationException( $"Component '{typeof( T )}' was not found" );
                return Addressables.ResourceManager.CreateCompletedOperation( result, null );
            } );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadPrefabListAsync<T>(string[] keys) where T : UnityEngine.Object {
            return Addressables.LoadAssetsAsync<GameObject>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).Chain( dependency => {
                var result = (IReadOnlyList<T>) dependency.Result.Select( i => i.GetComponent<T>() ?? throw new InvalidOperationException( $"Component '{typeof( T )}' was not found" ) ).ToList();
                return Addressables.ResourceManager.CreateCompletedOperation( result, null );
            } );
        }

        // Helpers
        private static AsyncOperationHandle<T> Chain<T, TDependency>(this AsyncOperationHandle<TDependency> dependency, Func<AsyncOperationHandle<TDependency>, AsyncOperationHandle<T>> operationProvider) {
            return Addressables.ResourceManager.CreateChainOperation( dependency, dependency => {
                if (dependency.IsSucceeded()) {
                    var operation = operationProvider( dependency );
                    operation.Destroyed += i => Addressables.Release( dependency );
                    return operation;
                } else {
                    var operation = Addressables.ResourceManager.CreateCompletedOperationWithException<T>( default!, dependency.OperationException );
                    operation.Destroyed += i => Addressables.Release( dependency );
                    return operation;
                }
            } );
        }

    }
}

#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    internal static class AddressableHandleHelper {

        // LoadAssetAsync
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object {
            return Addressables.LoadAssetAsync<T>( key );
        }
        public static AsyncOperationHandle<IReadOnlyList<T>> LoadAssetListAsync<T>(string[] keys) where T : UnityEngine.Object {
            return Addressables.LoadAssetsAsync<T>( keys.AsEnumerable(), null, Addressables.MergeMode.Union ).ChainOperation( assetsHandle => {
                return Addressables.ResourceManager.CreateCompletedOperation( (IReadOnlyList<T>) assetsHandle.Result, null );
            } );
        }

        // Helpers
        private static AsyncOperationHandle<T> ChainOperation<TDependency, T>(this AsyncOperationHandle<TDependency> dependency, Func<AsyncOperationHandle<TDependency>, AsyncOperationHandle<T>> operationProvider) {
            return Addressables.ResourceManager.CreateChainOperation( dependency, dependency => {
                if (dependency.IsSucceeded()) {
                    var operation = operationProvider( dependency );
                    operation.Destroyed += i => {
                        Addressables.Release( dependency );
                    };
                    return operation;
                } else {
                    var operation = Addressables.ResourceManager.CreateCompletedOperationWithException<T>( default!, dependency.OperationException );
                    operation.Destroyed += i => {
                        Addressables.Release( dependency );
                    };
                    return operation;
                }
            } );
        }

    }
}

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

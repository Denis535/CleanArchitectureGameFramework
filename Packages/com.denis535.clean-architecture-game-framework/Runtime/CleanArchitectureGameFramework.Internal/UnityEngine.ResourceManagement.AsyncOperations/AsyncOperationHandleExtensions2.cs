#nullable enable
namespace UnityEngine.ResourceManagement.AsyncOperations {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class AsyncOperationHandleExtensions2 {

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
            handle.WaitForCompletion();
            if (handle.IsValid() && handle.IsSucceeded()) {
                return;
            }
            throw handle.OperationException;
        }
        public static async Task WaitAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            await handle.Task.WaitAsync( cancellationToken );
            if (handle.IsValid() && handle.IsSucceeded()) {
                return;
            }
            throw handle.OperationException;
        }

        // GetResult
        public static T GetResult<T>(this AsyncOperationHandle<T> handle) {
            handle.WaitForCompletion();
            if (handle.IsValid() && handle.IsSucceeded()) {
                return handle.Result;
            }
            throw handle.OperationException;
        }
        public static async Task<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken) {
            await handle.Task.WaitAsync( cancellationToken );
            if (handle.IsValid() && handle.IsSucceeded()) {
                return handle.Result;
            }
            throw handle.OperationException;
        }

    }
}

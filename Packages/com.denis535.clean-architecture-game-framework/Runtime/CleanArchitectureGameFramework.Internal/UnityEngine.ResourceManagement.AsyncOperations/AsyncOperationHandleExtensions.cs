#nullable enable
namespace UnityEngine.ResourceManagement.AsyncOperations {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class AsyncOperationHandleExtensions {

        // IsState
        public static bool IsNone(this AsyncOperationHandle handle) {
            return handle.Status == AsyncOperationStatus.None;
        }
        public static bool IsSucceeded(this AsyncOperationHandle handle) {
            return handle.Status == AsyncOperationStatus.Succeeded;
        }
        public static bool IsFailed(this AsyncOperationHandle handle) {
            return handle.Status == AsyncOperationStatus.Failed;
        }

        // Wait
        public static void Wait(this AsyncOperationHandle handle) {
            handle.WaitForCompletion();
            if (handle.IsSucceeded()) {
                return;
            }
            throw handle.OperationException;
        }
        public static async Task WaitAsync(this AsyncOperationHandle handle, CancellationToken cancellationToken) {
            await handle.Task.WaitAsync( cancellationToken );
            if (handle.IsSucceeded()) {
                return;
            }
            throw handle.OperationException;
        }

        // GetResult
        public static object GetResult(this AsyncOperationHandle handle) {
            handle.WaitForCompletion();
            if (handle.IsSucceeded()) {
                return handle.Result;
            }
            throw handle.OperationException;
        }
        public static async Task<object> GetResultAsync(this AsyncOperationHandle handle, CancellationToken cancellationToken) {
            await handle.Task.WaitAsync( cancellationToken );
            if (handle.IsSucceeded()) {
                return handle.Result;
            }
            throw handle.OperationException;
        }

    }
}

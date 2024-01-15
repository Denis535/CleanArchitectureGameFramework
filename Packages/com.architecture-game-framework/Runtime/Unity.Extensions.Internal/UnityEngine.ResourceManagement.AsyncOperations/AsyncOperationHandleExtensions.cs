#nullable enable
namespace UnityEngine.ResourceManagement.AsyncOperations {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class AsyncOperationHandleExtensions {

        // IsSucceeded
        public static bool IsSucceeded(this AsyncOperationHandle handle) {
            return handle.Status == AsyncOperationStatus.Succeeded;
        }
        public static bool IsSucceeded<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.Succeeded;
        }

        // IsFailed
        public static bool IsFailed(this AsyncOperationHandle handle) {
            return handle.Status == AsyncOperationStatus.Failed;
        }
        public static bool IsFailed<T>(this AsyncOperationHandle<T> handle) {
            return handle.Status == AsyncOperationStatus.Failed;
        }

        // Wait
        public static void Wait(this AsyncOperationHandle handle) {
            handle.WaitForCompletion();
        }
        public static void Wait<T>(this AsyncOperationHandle<T> handle) {
            handle.WaitForCompletion();
        }

        // GetResult
        public static object GetResult(this AsyncOperationHandle handle) {
            return handle.WaitForCompletion();
        }
        public static T GetResult<T>(this AsyncOperationHandle<T> handle) {
            return handle.WaitForCompletion();
        }

        // Wait/Async
        public static Task WaitAsync(this AsyncOperationHandle handle, CancellationToken cancellationToken, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle>? onCancel = null) {
            try {
                var result = handle.Task.WaitAsync( cancellationToken );
                onComplete?.Invoke( handle );
                return result;
            } catch (OperationCanceledException) {
                onCancel?.Invoke( handle );
                throw;
            }
        }
        public static Task WaitAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken, Action<AsyncOperationHandle<T>>? onComplete = null, Action<AsyncOperationHandle<T>>? onCancel = null) {
            try {
                var result = handle.Task.WaitAsync( cancellationToken );
                onComplete?.Invoke( handle );
                return result;
            } catch (OperationCanceledException) {
                onCancel?.Invoke( handle );
                throw;
            }
        }

        // GetResult/Async
        public static Task<object> GetResultAsync(this AsyncOperationHandle handle, CancellationToken cancellationToken, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle>? onCancel = null) {
            try {
                var result = handle.Task.WaitAsync( cancellationToken );
                onComplete?.Invoke( handle );
                return result;
            } catch (OperationCanceledException) {
                onCancel?.Invoke( handle );
                throw;
            }
        }
        public static Task<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken, Action<AsyncOperationHandle<T>>? onComplete = null, Action<AsyncOperationHandle<T>>? onCancel = null) {
            try {
                var result = handle.Task.WaitAsync( cancellationToken );
                onComplete?.Invoke( handle );
                return result;
            } catch (OperationCanceledException) {
                onCancel?.Invoke( handle );
                throw;
            }
        }

    }
}

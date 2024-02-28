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
        public static void Wait(this AsyncOperationHandle handle, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle, Exception>? onError = null) {
            try {
                handle.WaitForCompletion();
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }
        public static void Wait<T>(this AsyncOperationHandle<T> handle, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle, Exception>? onError = null) {
            try {
                handle.WaitForCompletion();
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }
        // Wait/Async
        public static async Task WaitAsync(this AsyncOperationHandle handle, CancellationToken cancellationToken, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle, Exception>? onError = null) {
            try {
                await handle.Task.WaitAsync( cancellationToken );
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }
        public static async Task WaitAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken, Action<AsyncOperationHandle<T>>? onComplete = null, Action<AsyncOperationHandle<T>, Exception>? onError = null) {
            try {
                await handle.Task.WaitAsync( cancellationToken );
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }

        // GetResult
        public static object GetResult(this AsyncOperationHandle handle, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle, Exception>? onError = null) {
            try {
                handle.WaitForCompletion();
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
                return handle.Result;
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }
        public static T GetResult<T>(this AsyncOperationHandle<T> handle, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle, Exception>? onError = null) {
            try {
                handle.WaitForCompletion();
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
                return handle.Result;
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }
        // GetResult/Async
        public static async Task<object> GetResultAsync(this AsyncOperationHandle handle, CancellationToken cancellationToken, Action<AsyncOperationHandle>? onComplete = null, Action<AsyncOperationHandle, Exception>? onError = null) {
            try {
                await handle.Task.WaitAsync( cancellationToken );
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
                return handle.Result;
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }
        public static async Task<T> GetResultAsync<T>(this AsyncOperationHandle<T> handle, CancellationToken cancellationToken, Action<AsyncOperationHandle<T>>? onComplete = null, Action<AsyncOperationHandle<T>, Exception>? onError = null) {
            try {
                await handle.Task.WaitAsync( cancellationToken );
                if (handle.IsValid() && handle.IsFailed()) throw handle.OperationException;
                onComplete?.Invoke( handle );
                return handle.Result;
            } catch (Exception ex) {
                onError?.Invoke( handle, ex );
                throw;
            }
        }

    }
}

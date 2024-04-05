#nullable enable
namespace UnityEngine.ResourceManagement.AsyncOperations {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public static class AsyncOperationExtensions {

        // Wait
        public static void Wait<T>(this AsyncOperationBase<T> operation, Action<AsyncOperationBase<T>>? onComplete = null, Action<AsyncOperationBase<T>, Exception>? onError = null) {
            try {
                operation.WaitForCompletion();
                onComplete?.Invoke( operation );
            } catch (Exception ex) {
                onError?.Invoke( operation, ex );
                throw;
            }
        }
        public static async Task WaitAsync<T>(this AsyncOperationBase<T> operation, CancellationToken cancellationToken, Action<AsyncOperationBase<T>>? onComplete = null, Action<AsyncOperationBase<T>, Exception>? onError = null) {
            try {
                await operation.WaitForCompletionAsync( cancellationToken );
                onComplete?.Invoke( operation );
            } catch (Exception ex) {
                onError?.Invoke( operation, ex );
                throw;
            }
        }

        // GetResult
        public static T GetResult<T>(this AsyncOperationBase<T> operation, Action<AsyncOperationBase<T>>? onComplete = null, Action<AsyncOperationBase<T>, Exception>? onError = null) {
            try {
                operation.WaitForCompletion();
                onComplete?.Invoke( operation );
                return operation.Result;
            } catch (Exception ex) {
                onError?.Invoke( operation, ex );
                throw;
            }
        }
        public static async Task<T> GetResultAsync<T>(this AsyncOperationBase<T> operation, CancellationToken cancellationToken, Action<AsyncOperationBase<T>>? onComplete = null, Action<AsyncOperationBase<T>, Exception>? onError = null) {
            try {
                await operation.WaitForCompletionAsync( cancellationToken );
                onComplete?.Invoke( operation );
                return operation.Result;
            } catch (Exception ex) {
                onError?.Invoke( operation, ex );
                throw;
            }
        }

        // Helpers
        private static async Task WaitForCompletionAsync<T>(this AsyncOperationBase<T> operation, CancellationToken cancellationToken) {
            cancellationToken.ThrowIfCancellationRequested();
            while (operation.IsRunning) {
                await Task.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

    }
}

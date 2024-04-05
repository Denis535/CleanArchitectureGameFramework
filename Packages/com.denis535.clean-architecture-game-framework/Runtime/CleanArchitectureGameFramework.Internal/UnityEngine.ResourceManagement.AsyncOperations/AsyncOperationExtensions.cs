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
        public static void Wait<T>(this AsyncOperationBase<T> operation) {
            operation.WaitForCompletion();
        }
        public static async Task WaitAsync<T>(this AsyncOperationBase<T> operation, CancellationToken cancellationToken) {
            await operation.WaitForCompletionAsync( cancellationToken );
        }

        // GetResult
        public static T GetResult<T>(this AsyncOperationBase<T> operation) {
            operation.WaitForCompletion();
            return operation.Result;
        }
        public static async Task<T> GetResultAsync<T>(this AsyncOperationBase<T> operation, CancellationToken cancellationToken) {
            await operation.WaitForCompletionAsync( cancellationToken );
            return operation.Result;
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

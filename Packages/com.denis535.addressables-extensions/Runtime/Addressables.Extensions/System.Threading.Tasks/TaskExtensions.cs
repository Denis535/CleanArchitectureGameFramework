#nullable enable
namespace System.Threading.Tasks {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class TaskExtensions {

        // WaitAsync
        public static async Task WaitAsync(this Task task, CancellationToken cancellationToken) {
            if (task.IsCompleted) {
                await task.ConfigureAwait( false );
                return;
            }
            if (!cancellationToken.CanBeCanceled) {
                await task.ConfigureAwait( false );
                return;
            }
            if (cancellationToken.IsCancellationRequested) {
                throw new OperationCanceledException( cancellationToken );
            }
            using (cancellationToken.WaitAsync( out var cancellationTask )) {
                var anyTask = await Task.WhenAny( task, cancellationTask ).ConfigureAwait( false );
                if (anyTask == cancellationTask) {
                    throw new OperationCanceledException( cancellationToken );
                }
            }
            await task.ConfigureAwait( false );
        }

        // WaitAsync
        public static async Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken) {
            if (task.IsCompleted) {
                return await task.ConfigureAwait( false );
            }
            if (!cancellationToken.CanBeCanceled) {
                return await task.ConfigureAwait( false );
            }
            if (cancellationToken.IsCancellationRequested) {
                throw new OperationCanceledException( cancellationToken );
            }
            using (cancellationToken.WaitAsync( out var cancellationTask )) {
                var anyTask = await Task.WhenAny( task, cancellationTask ).ConfigureAwait( false );
                if (anyTask == cancellationTask) {
                    throw new OperationCanceledException( cancellationToken );
                }
            }
            return await task.ConfigureAwait( false );
        }

        // Throw
        public static async void Throw(this Task task) {
            await task;
        }

        // Helpers
        private static CancellationTokenRegistration WaitAsync(this CancellationToken cancellationToken, out Task task) {
            var tcs = new TaskCompletionSource<object?>();
            task = tcs.Task;
            return cancellationToken.Register( tcs => ((TaskCompletionSource<object?>) tcs).SetResult( null ), tcs );
        }

    }
}

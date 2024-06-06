#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class DisposableExtensions {

        // DisposeAll
        public static void DisposeAll(this IEnumerable<Disposable> disposables) {
            foreach (var disposable in disposables) {
                Assert.Operation.Message( $"Disposable {disposable} must be non-disposed" ).NotDisposed( !disposable.IsDisposed );
                disposable.IsDisposed = true;
                disposable.disposeCancellationTokenSource?.Cancel();
            }
        }

        // ThrowIfDisposed
        public static void ThrowIfDisposed(this Disposable disposable) {
            Assert.Operation.Message( $"Disposable {disposable} must be non-disposed" ).NotDisposed( !disposable.IsDisposed );
        }

        // IfNotDisposed
        public static T IfNotDisposed<T>(this T disposable) where T : Disposable {
            disposable.ThrowIfDisposed();
            return disposable;
        }

    }
}

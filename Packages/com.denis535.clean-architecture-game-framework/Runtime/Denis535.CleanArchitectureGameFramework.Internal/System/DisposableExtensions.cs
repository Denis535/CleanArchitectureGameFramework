#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class DisposableExtensions {

        // DisposeAll
        public static void DisposeAll(this IEnumerable<DisposableBase> disposables) {
            foreach (var disposable in disposables) {
                disposable.Dispose();
            }
        }

        // ThrowIfDisposed
        public static void ThrowIfDisposed(this DisposableBase disposable) {
            Assert.Operation.Message( $"Disposable {disposable} must be non-disposed" ).NotDisposed( !disposable.IsDisposed );
        }

    }
}

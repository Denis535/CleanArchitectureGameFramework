#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class DisposableExtensions {

        // ThrowIfDisposed
        public static void ThrowIfDisposed(this Disposable disposable) {
            Assert.Object.Message( $"Disposable {disposable} must not be disposed" ).NotDisposed( !disposable.IsDisposed );
        }

        // IfNotDisposed
        public static T IfNotDisposed<T>(this T disposable) where T : Disposable {
            disposable.ThrowIfDisposed();
            return disposable;
        }

    }
}

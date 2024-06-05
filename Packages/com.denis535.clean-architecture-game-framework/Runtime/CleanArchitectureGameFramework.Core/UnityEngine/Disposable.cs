#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public abstract class Disposable : IDisposable {

        private CancellationTokenSource? disposeCancellationTokenSource;

        // System
        public bool IsDisposed { get; private set; }
        public CancellationToken DisposeCancellationToken {
            get {
                if (disposeCancellationTokenSource == null) {
                    disposeCancellationTokenSource = new CancellationTokenSource();
                    if (IsDisposed) disposeCancellationTokenSource.Cancel();
                }
                return disposeCancellationTokenSource.Token;
            }
        }

        // Constructor
        public Disposable() {
        }
        public virtual void Dispose() {
            this.ThrowIfDisposed();
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

        // Helpers
        protected static void Dispose(Disposable disposable) {
            disposable.ThrowIfDisposed();
            disposable.IsDisposed = true;
            disposable.disposeCancellationTokenSource?.Cancel();
        }

    }
}

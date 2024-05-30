#nullable enable
namespace System {
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
            Assert.Object.Message( $"Disposable {this} must not be disposed" ).NotDisposed( !IsDisposed );
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

        // Helpers
        protected void DisposeInternal() {
            Assert.Object.Message( $"Disposable {this} must not be disposed" ).NotDisposed( !IsDisposed );
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

    }
}

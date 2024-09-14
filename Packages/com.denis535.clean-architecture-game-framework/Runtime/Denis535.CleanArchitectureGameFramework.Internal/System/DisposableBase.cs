#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public abstract class DisposableBase : IDisposable {

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
        public DisposableBase() {
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"Disposable {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }
        protected void DisposeInternal() {
            Assert.Operation.Message( $"Disposable {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

    }
}

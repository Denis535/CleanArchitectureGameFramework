#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    public abstract class PlayerBase : IDisposable {

        protected CancellationTokenSource? disposeCancellationTokenSource;

        // System
        public bool IsDisposed { get; protected set; }
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
        public PlayerBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"Player {this} must be alive" ).NotDisposed( !IsDisposed );
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : IDisposable {

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
        // VisualElement
        protected internal VisualElement VisualElement { get; protected init; } = default!;
        // Children
        public virtual IReadOnlyList<UIViewBase> Children => Array.Empty<UIViewBase>();

        // Constructor
        public UIViewBase() {
        }
        public UIViewBase(VisualElement visualElement) {
            VisualElement = visualElement;
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"View {this} must be alive" ).Alive( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( VisualElement.panel == null );
            foreach (var child in Children) {
                child.Dispose();
            }
            Assert.Operation.Message( $"View {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

    }
}

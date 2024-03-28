#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : IDisposable {

        protected CancellationTokenSource? disposeCancellationTokenSource;
        private VisualElement visualElement = default!;

        // System
        public static Action<UIViewBase, VisualElement>? InitializeDelegate { get; set; } = (UIViewBase view, VisualElement element) => {
            element.OnAttachToPanel( evt => {
                ViewAttachEvent.Dispatch( element, view );
            } );
            element.OnDetachFromPanel( evt => {
                ViewDetachEvent.Dispatch( element, view );
            } );
        };

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
        // View
        protected internal VisualElement VisualElement {
            get => visualElement;
            protected init {
                visualElement = value;
                InitializeDelegate?.Invoke( this, value );
            }
        }

        // Constructor
        public UIViewBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"View {this} must be alive" ).Alive( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( VisualElement.panel == null );
            if (VisualElement.visualTreeAssetSource != null) Addressables2.Release( VisualElement.visualTreeAssetSource );
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

    }
}

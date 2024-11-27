#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : VisualElement, IDisposable {

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
        // IsAttachedToPanel
        public bool IsAttachedToPanel {
            get {
                Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return panel != null;
            }
        }
        // IsAttachedToParent
        public bool IsAttachedToParent {
            get {
                Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return parent != null;
            }
        }
        // Parent
        public UIViewBase? Parent2 {
            get {
                Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return GetParent( this );
                static UIViewBase? GetParent(VisualElement element) {
                    if (element.parent != null) {
                        return (element.parent as UIViewBase) ?? GetParent( element.parent );
                    }
                    return null;
                }
            }
        }
        // Children
        public IEnumerable<UIViewBase> Children2 {
            get {
                Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return GetChildren( this );
                static IEnumerable<UIViewBase> GetChildren(VisualElement element) {
                    foreach (var child in element.Children()) {
                        if (child is UIViewBase child_) {
                            yield return child_;
                        } else {
                            foreach (var i in GetChildren( child )) yield return i;
                        }
                    }
                }
            }
        }

        // Constructor
        public UIViewBase() {
        }
        ~UIViewBase() {
            if (!IsDisposed) {
                Debug.LogWarning( $"View '{this}' must be disposed" );
            }
        }
        public virtual void Dispose() {
            foreach (var child in Children2) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached to panel" ).Valid( !IsAttachedToPanel );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

        // AddView
        protected internal virtual bool AddView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            return false;
        }
        protected internal virtual bool RemoveView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            return false;
        }

    }
}

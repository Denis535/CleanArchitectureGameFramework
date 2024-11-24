#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : VisualElement {

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
        public bool IsAttachedToPanel => panel != null;
        // IsAttachedToParent
        public bool IsAttachedToParent => parent != null;
        // Parent
        public UIViewBase? Parent2 {
            get {
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
        public virtual void Dispose() {
            foreach (var child in Children2) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached to panel" ).Valid( !IsAttachedToPanel );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

        // AddViewRecursive
        internal void AddViewRecursive(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (TryAddViewRecursive( view )) {
                return;
            }
            Assert.Operation.Message( $"View {view} was not added" ).Valid( view.IsAttachedToParent );
        }
        internal void RemoveViewRecursive(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (TryRemoveViewRecursive( view )) {
                return;
            }
            Assert.Operation.Message( $"View {view} was not removed" ).Valid( !view.IsAttachedToParent );
        }

        // TryAddViewRecursive
        private bool TryAddViewRecursive(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (TryAddView( view )) {
                return true;
            }
            return Parent2?.TryAddViewRecursive( view ) ?? false;
        }
        private bool TryRemoveViewRecursive(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (TryRemoveView( view )) {
                return true;
            }
            return Parent2?.TryRemoveViewRecursive( view ) ?? false;
        }

        // TryAddView
        protected virtual bool TryAddView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            return false;
        }
        protected virtual bool TryRemoveView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            return false;
        }

    }
}

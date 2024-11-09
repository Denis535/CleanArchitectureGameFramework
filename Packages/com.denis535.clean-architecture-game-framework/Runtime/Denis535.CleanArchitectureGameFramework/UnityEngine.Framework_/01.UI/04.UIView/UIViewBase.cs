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
        // IsAttached
        public bool IsAttached => panel != null;
        // IsShown
        public bool IsShown => parent != null;
        // Parent
        public UIViewBase? Parent2 {
            get {
                return GetParent( this );
                static UIViewBase? GetParent(VisualElement element) {
                    if (element.parent is UIViewBase parent_) {
                        return parent_;
                    }
                    if (element.parent != null) {
                        return GetParent( element.parent );
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
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( !IsAttached );
            foreach (var child in Children2) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

        // AddViewRecursive
        public void AddViewRecursive(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-shown" ).Valid( !view.IsShown );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (AddViewRecursive( this, view )) {
                return;
            }
            Assert.Operation.Message( $"Can not add {view} view" ).Valid( view.IsShown );
        }
        public void RemoveViewRecursive(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be shown" ).Valid( view.IsShown );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (RemoveViewRecursive( this, view )) {
                return;
            }
            Assert.Operation.Message( $"Can not remove {view} view" ).Valid( !view.IsShown );
        }

        // AddView
        protected virtual bool AddView(UIViewBase view) {
            return false;
        }
        protected virtual bool RemoveView(UIViewBase view) {
            return false;
        }

        // Helpers
        private static bool AddViewRecursive(VisualElement element, UIViewBase view) {
            if (element is UIViewBase element_ && element_.AddView( view )) {
                return true;
            }
            if (element.parent != null) {
                return AddViewRecursive( element.parent, view );
            }
            return false;
        }
        private static bool RemoveViewRecursive(VisualElement element, UIViewBase view) {
            if (element is UIViewBase element_ && element_.RemoveView( view )) {
                return true;
            }
            if (element.parent != null) {
                return RemoveViewRecursive( element.parent, view );
            }
            return false;
        }

    }
}

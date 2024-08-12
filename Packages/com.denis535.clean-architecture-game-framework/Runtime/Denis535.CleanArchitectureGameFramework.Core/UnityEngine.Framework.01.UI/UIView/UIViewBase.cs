#nullable enable
namespace UnityEngine.Framework.UI {
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
        public UIViewBase(string name, params string[] classes) {
            this.name = name;
            foreach (var @class in classes) {
                AddToClassList( @class );
            }
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( !IsAttached );
            foreach (var child in Children2) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

        // ShowView
        public void ShowView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-shown" ).Valid( !view.IsShown );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (AddViewRecursive( view )) {
                return;
            }
            Assert.Operation.Message( $"Can not show view {view}" ).Valid( view.IsShown );
        }
        public void HideView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be shown" ).Valid( view.IsShown );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (RemoveViewRecursive( view )) {
                return;
            }
            Assert.Operation.Message( $"Can not hide view {view}" ).Valid( !view.IsShown );
        }

        // AddViewRecursive
        private bool AddViewRecursive(UIViewBase view) {
            return AddView( view ) || (Parent2?.AddViewRecursive( view ) ?? false);
        }
        private bool RemoveViewRecursive(UIViewBase view) {
            return RemoveView( view ) || (Parent2?.RemoveViewRecursive( view ) ?? false);
        }

        // AddView
        protected virtual bool AddView(UIViewBase view) {
            return false;
        }
        protected virtual bool RemoveView(UIViewBase view) {
            return false;
        }

    }
}

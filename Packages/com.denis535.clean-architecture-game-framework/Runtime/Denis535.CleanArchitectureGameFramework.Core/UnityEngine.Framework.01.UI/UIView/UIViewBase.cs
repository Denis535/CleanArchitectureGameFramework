#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : VisualElement, IDisposable {

        private CancellationTokenSource? disposeCancellationTokenSource;
        private VisualElement? focusedElement;

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
            foreach (var child in GetChildren()) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

        // GetParent
        public UIViewBase? GetParent() {
            return GetParent( this );
            static UIViewBase? GetParent(VisualElement element) {
                if (element.parent is VisualElement parent) {
                    if (parent is UIViewBase parent_) {
                        return parent_;
                    } else {
                        return GetParent( parent );
                    }
                }
                return null;
            }
        }
        public IEnumerable<UIViewBase> GetChildren() {
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

        // LoadFocusedElement
        public VisualElement? LoadFocusedElement() {
            return focusedElement;
        }
        public void SaveFocusedElement(VisualElement? focusedElement) {
            this.focusedElement = focusedElement;
        }

    }
}

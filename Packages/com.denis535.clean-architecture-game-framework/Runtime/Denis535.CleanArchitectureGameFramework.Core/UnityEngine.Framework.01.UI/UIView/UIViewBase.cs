#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : VisualElement, IDisposable {

        private VisualElement? focusedElement;

        // System
        public bool IsDisposed { get; private set; }
        // IsAttached
        internal bool IsAttached => panel != null;
        // IsShown
        internal bool IsShown => parent != null;

        // Constructor
        public UIViewBase() {
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( !IsAttached );
            foreach (var child in GetChildren()) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            IsDisposed = true;
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

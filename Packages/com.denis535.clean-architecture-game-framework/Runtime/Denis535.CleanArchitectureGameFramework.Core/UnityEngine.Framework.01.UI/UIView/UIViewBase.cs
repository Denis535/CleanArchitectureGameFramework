#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : Disposable {

        private VisualElement? focusedElement;

        // VisualElement
        protected internal abstract VisualElement VisualElement { get; }
        // IsAttached
        internal bool IsAttached => VisualElement.IsAttached();
        // IsShown
        internal bool IsShown => VisualElement.parent != null;
        // Parent
        internal UIViewBase? Parent => GetParent( this );
        // Children
        internal IEnumerable<UIViewBase> Children => GetChildren( this );

        // Constructor
        public UIViewBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( !IsAttached );
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            base.Dispose();
        }

        // LoadFocusedElement
        public VisualElement? LoadFocusedElement() {
            return focusedElement;
        }
        public void SaveFocusedElement(VisualElement? focusedElement) {
            this.focusedElement = focusedElement;
        }

        // Helpers
        private static UIViewBase? GetParent(UIViewBase view) {
            return GetParent( view.VisualElement );
        }
        private static IEnumerable<UIViewBase> GetChildren(UIViewBase view) {
            return GetChildren( view.VisualElement );
        }
        // Helpers
        private static UIViewBase? GetParent(VisualElement element) {
            if (element.parent is VisualElement parent) {
                if (parent.userData is UIViewBase) {
                    return (UIViewBase) parent.userData;
                } else {
                    return GetParent( parent );
                }
            }
            return null;
        }
        private static IEnumerable<UIViewBase> GetChildren(VisualElement element) {
            foreach (var child in element.Children()) {
                if (child.userData is UIViewBase) {
                    yield return (UIViewBase) child.userData;
                } else {
                    foreach (var i in GetChildren( child )) yield return i;
                }
            }
        }

    }
}

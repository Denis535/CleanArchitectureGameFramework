#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase2 : UIViewBase {

        private VisualElement? focusedElement;

        // VisualElement
        protected internal abstract VisualElement VisualElement { get; }
        // IsAttached
        internal override bool IsAttached => VisualElement.IsAttached();
        // IsShown
        internal override bool IsShown => VisualElement.parent != null;
        // Parent
        internal override UIViewBase? Parent => GetParent( this );
        // Children
        internal override IEnumerable<UIViewBase> Children => GetChildren( this );

        // Constructor
        public UIViewBase2() {
        }
        public override void Dispose() {
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
        private static UIViewBase? GetParent(UIViewBase2 view) {
            return GetParent( view.VisualElement );
        }
        private static IEnumerable<UIViewBase> GetChildren(UIViewBase2 view) {
            return GetChildren( view.VisualElement );
        }
        // Helpers
        private static UIViewBase? GetParent(VisualElement element) {
            if (element.parent is VisualElement parent) {
                if (parent.userData is UIViewBase2) {
                    return (UIViewBase2) parent.userData;
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

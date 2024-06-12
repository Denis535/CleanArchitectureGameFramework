#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : Disposable {

        private VisualElement visualElement = default!;
        private VisualElement? focusedElement;

        // Layer
        public abstract int Layer { get; }
        // VisualElement
        protected internal VisualElement VisualElement {
            internal get => visualElement;
            init {
                visualElement = value;
                visualElement.userData = this;
            }
        }

        // Constructor
        public UIViewBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( !VisualElement.IsAttached() );
            Assert.Operation.Message( $"View {this} children must be disposed" ).Valid( GetChildren().All( i => i.IsDisposed ) );
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
        protected IEnumerable<UIViewBase> GetChildren() {
            return GetChildren( visualElement );
            static IEnumerable<UIViewBase> GetChildren(VisualElement element) {
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
}

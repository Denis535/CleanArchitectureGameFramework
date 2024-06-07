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

        // VisualElement
        protected internal VisualElement VisualElement {
            internal get => visualElement;
            init {
                visualElement = value;
                visualElement.userData = this;
            }
        }
        // Priority
        public abstract int Priority { get; }
        // IsAlwaysVisible
        public abstract bool IsAlwaysVisible { get; }
        // IsModal
        public abstract bool IsModal { get; }

        // Constructor
        public UIViewBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} children must be disposed" ).Valid( this.GetChildren().All( i => i.IsDisposed ) );
            base.Dispose();
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

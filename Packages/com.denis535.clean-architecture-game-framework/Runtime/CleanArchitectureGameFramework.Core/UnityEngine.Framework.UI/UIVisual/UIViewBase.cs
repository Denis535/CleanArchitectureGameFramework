#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
            Assert.Operation.Message( $"View {this} must not be disposed" ).NotDisposed( !IsDisposed );
            this.GetChildren().DisposeAll();
            base.Dispose();
        }

        // Focus
        public virtual void Focus() {
            if (visualElement.focusable) {
                visualElement.Focus();
            } else {
                visualElement.focusable = true;
                visualElement.delegatesFocus = true;
                visualElement.Focus();
                visualElement.delegatesFocus = false;
                visualElement.focusable = false;
            }
        }
        public virtual void SaveFocus() {
            var focusedElement = GetFocusedElement();
            SaveFocusedElement( focusedElement );
        }
        public virtual bool LoadFocus() {
            var focusedElement = LoadFocusedElement();
            if (focusedElement != null) {
                focusedElement.Focus();
                return true;
            }
            return false;
        }

        // GetFocusedElement
        public VisualElement? GetFocusedElement() {
            var focusedElement = (VisualElement) visualElement.focusController.focusedElement;
            if (focusedElement != null && (visualElement == focusedElement || visualElement.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        public bool HasFocusedElement() {
            var focusedElement = (VisualElement) visualElement.focusController.focusedElement;
            if (focusedElement != null && (visualElement == focusedElement || visualElement.Contains( focusedElement ))) return true;
            return false;
        }

        // SaveFocusedElement
        public void SaveFocusedElement(VisualElement? focusedElement) {
            this.focusedElement = focusedElement;
        }
        public VisualElement? LoadFocusedElement() {
            return focusedElement;
        }

    }
}

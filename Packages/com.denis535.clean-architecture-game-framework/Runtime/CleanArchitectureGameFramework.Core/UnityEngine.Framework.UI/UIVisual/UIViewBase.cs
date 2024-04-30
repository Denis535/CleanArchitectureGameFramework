#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : IDisposable {

        protected CancellationTokenSource? disposeCancellationTokenSource;
        private VisualElement? visualElement;
        private VisualElement? focusedElement;

        // System
        public bool IsDisposed { get; protected set; }
        public CancellationToken DisposeCancellationToken {
            get {
                if (disposeCancellationTokenSource == null) {
                    disposeCancellationTokenSource = new CancellationTokenSource();
                    if (IsDisposed) disposeCancellationTokenSource.Cancel();
                }
                return disposeCancellationTokenSource.Token;
            }
        }
        // VisualElement
        protected internal VisualElement VisualElement {
            get => visualElement!;
            protected init {
                Assert.Operation.Message( $"View {this} already have VisualElement" ).Valid( visualElement == null );
                visualElement = value;
                visualElement.userData = this;
            }
        }
        // Children
        public IReadOnlyList<UIViewBase> Children {
            get {
                return VisualElement.GetDescendants( i => i.userData == this || i.userData is not UIViewBase )
                    .Select( i => i.userData )
                    .OfType<UIViewBase>()
                    .ToList();
            }
        }

        // Constructor
        public UIViewBase() {
        }
        public UIViewBase(VisualElement visualElement) {
            VisualElement = visualElement;
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"View {this} must be alive" ).Alive( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( VisualElement.panel == null );
            foreach (var child in Children) {
                child.Dispose();
            }
            Assert.Operation.Message( $"View {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

        // Focus
        public virtual void Focus() {
            if (VisualElement.focusable) {
                VisualElement.Focus();
            } else {
                VisualElement.focusable = true;
                VisualElement.delegatesFocus = true;
                VisualElement.Focus();
                VisualElement.delegatesFocus = false;
                VisualElement.focusable = false;
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
        public bool HasFocusedElement() {
            var focusedElement = (VisualElement) VisualElement.focusController.focusedElement;
            if (focusedElement != null && (VisualElement == focusedElement || VisualElement.Contains( focusedElement ))) return true;
            return false;
        }
        public VisualElement? GetFocusedElement() {
            var focusedElement = (VisualElement) VisualElement.focusController.focusedElement;
            if (focusedElement != null && (VisualElement == focusedElement || VisualElement.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        private void SaveFocusedElement(VisualElement? focusedElement) {
            this.focusedElement = focusedElement;
        }
        private VisualElement? LoadFocusedElement() {
            return focusedElement;
        }

    }
}

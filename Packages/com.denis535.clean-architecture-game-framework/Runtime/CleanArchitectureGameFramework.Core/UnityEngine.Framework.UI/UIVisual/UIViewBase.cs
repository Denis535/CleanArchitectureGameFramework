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
        private VisualElement visualElement = default!;
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
            internal get => visualElement;
            init {
                visualElement = value;
                visualElement.userData = this;
            }
        }
        // Priority
        public virtual int Priority => 0;
        // IsAlwaysVisible
        public virtual bool IsAlwaysVisible => false;
        // IsModal
        public virtual bool IsModal => false;

        // Constructor
        public UIViewBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"View {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( visualElement.panel == null );
            foreach (var child in this.GetChildren()) {
                child.Dispose();
            }
#if UNITY_EDITOR
            Assert.Operation.Message( $"View {this} children must be disposed" ).Valid( this.GetChildren().All( i => i.IsDisposed ) );
#endif
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
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

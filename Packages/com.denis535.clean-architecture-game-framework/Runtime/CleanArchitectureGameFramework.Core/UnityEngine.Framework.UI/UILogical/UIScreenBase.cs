#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    [DefaultExecutionOrder( ScriptExecutionOrders.UIScreen )]
    public abstract class UIScreenBase : MonoBehaviour, IUILogicalElement {
        
        private readonly Lock @lock = new Lock();

        // Document
        protected UIDocument Document { get; set; } = default!;
        // Widget
        public UIWidgetBase? Widget { get; private set; }

        // Awake
        public void Awake() {
            Document = gameObject.RequireComponentInChildren<UIDocument>();
        }
        public void OnDestroy() {
        }

        // AttachWidget
        public virtual void AttachWidget(UIWidgetBase widget, object? argument = null) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            using (@lock.Enter()) {
                Widget = widget;
                widget.Parent = null;
                if (true) {
                    UIWidgetBase.AttachToScreen( Widget, this, argument );
                }
            }
        }
        public virtual void DetachWidget(UIWidgetBase widget, object? argument = null) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
            Assert.Operation.Message( $"Screen {this} must have widget {widget} widget" ).Valid( Widget == widget );
            using (@lock.Enter()) {
                if (true) {
                    UIWidgetBase.DetachFromScreen( Widget, this, argument );
                }
                widget.Parent = null;
                Widget = null;
            }
            if (widget.DisposeWhenDetach) {
                widget.Dispose();
            }
        }

        // Helpers/AddView
        protected static void AddView(UIDocument document, UIViewBase view) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'document' {document} must have rootVisualElement" ).Valid( document.rootVisualElement != null );
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            document.rootVisualElement.Add( view.VisualElement );
        }
        protected static void AddViewIfNeeded(UIDocument document, UIViewBase view) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'document' {document} must have rootVisualElement" ).Valid( document.rootVisualElement != null );
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            if (!document.rootVisualElement.Contains( view.VisualElement )) {
                document.rootVisualElement.Add( view.VisualElement );
            }
        }
        protected static void RemoveView(UIDocument document, UIViewBase view) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'document' {document} must have rootVisualElement" ).Valid( document.rootVisualElement != null );
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            document.rootVisualElement.Remove( view.VisualElement );
        }

    }
}

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

        // Globals
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
        protected internal virtual void __AttachWidget__(UIWidgetBase widget, object? argument) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            using (@lock.Enter()) {
                Widget = widget;
                widget.Parent = null;
                if (true) {
                    UIWidgetBase.AttachToScreen( Widget, this, argument );
                }
            }
        }
        protected internal virtual void __DetachWidget__(UIWidgetBase widget, object? argument) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
            Assert.Object.Message( $"Screen {this} must have widget {widget} widget" ).Valid( Widget == widget );
            using (@lock.Enter()) {
                if (true) {
                    UIWidgetBase.DetachFromScreen( Widget, this, argument );
                }
                widget.Parent = null;
                Widget = null;
            }
            if (widget.DisposeAutomatically) {
                widget.Dispose();
            }
        }

        // Helpers/AddVisualElement
        protected static void AddVisualElement(UIDocument document, VisualElement element) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'document' {document} must have rootVisualElement" ).Valid( document.rootVisualElement != null );
            Assert.Argument.Message( $"Argument 'element' must be non-null" ).NotNull( element != null );
            document.rootVisualElement.Add( element );
        }
        protected static void AddVisualElementIfNeeded(UIDocument document, VisualElement element) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'document' {document} must have rootVisualElement" ).Valid( document.rootVisualElement != null );
            Assert.Argument.Message( $"Argument 'element' must be non-null" ).NotNull( element != null );
            if (!document.rootVisualElement.Contains( element )) {
                document.rootVisualElement.Add( element );
            }
        }
        protected static void RemoveVisualElement(UIDocument document, VisualElement element) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'document' {document} must have rootVisualElement" ).Valid( document.rootVisualElement != null );
            Assert.Argument.Message( $"Argument 'element' must be non-null" ).NotNull( element != null );
            document.rootVisualElement.Remove( element );
        }

    }
}

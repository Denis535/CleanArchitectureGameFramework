#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    [DefaultExecutionOrder( ScriptExecutionOrders.UIScreen )]
    public abstract class UIScreenBase : MonoBehaviour, IUILogicalElement {

        // System
        private Lock Lock { get; } = new Lock();
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
        protected internal virtual void __AttachWidget__(UIWidgetBase widget) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            using (Lock.Enter()) {
                Widget = widget;
                widget.Parent = null;
                UIWidgetBase.AttachToScreen( Widget, this );
            }
        }
        protected internal virtual void __DetachWidget__(UIWidgetBase widget) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
            Assert.Object.Message( $"Screen {this} must have widget {widget} widget" ).Valid( Widget == widget );
            using (Lock.Enter()) {
                UIWidgetBase.DetachFromScreen( Widget, this );
                widget.Parent = null;
                Widget = null;
            }
            if (widget.DisposeAutomatically) {
                widget.Dispose();
            }
        }

        // Helpers/AddView
        protected static void AddView(UIDocument document, VisualElement view) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            document.rootVisualElement.Add( view );
        }
        protected static void AddViewIfNeeded(UIDocument document, VisualElement view) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            if (!document.rootVisualElement.Contains( view )) {
                document.rootVisualElement.Add( view );
            }
        }
        protected static void RemoveView(UIDocument document, VisualElement view) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document is not null );
            Assert.Argument.Message( $"Argument 'document' {document} must be awakened" ).Valid( document.didAwake );
            Assert.Argument.Message( $"Argument 'document' {document} must be alive" ).Valid( document );
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            document.rootVisualElement.Remove( view );
        }

    }
}

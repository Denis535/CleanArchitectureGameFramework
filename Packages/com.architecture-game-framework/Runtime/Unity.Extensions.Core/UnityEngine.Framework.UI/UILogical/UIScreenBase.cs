#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase : MonoBehaviour, IUILogicalElement, IUIViewable {

        // System
        private Lock Lock { get; } = new Lock();
        // Globals
        protected internal UIDocument Document { get; set; } = default!;
        // View
        public UIScreenViewBase View { get; protected set; } = default!;
        UIViewBase IUIViewable.View => View;
        // Widget
        public UIWidgetBase? Widget { get; private set; }
        // OnDescendantWidgetAttach
        public Action<UIWidgetBase>? OnBeforeDescendantWidgetAttachEvent { get; set; }
        public Action<UIWidgetBase>? OnAfterDescendantWidgetAttachEvent { get; set; }
        public Action<UIWidgetBase>? OnBeforeDescendantWidgetDetachEvent { get; set; }
        public Action<UIWidgetBase>? OnAfterDescendantWidgetDetachEvent { get; set; }

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
                Widget.Attach( this );
            }
        }
        protected internal virtual void __DetachWidget__(UIWidgetBase widget) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
            Assert.Object.Message( $"Screen {this} must have widget {widget} widget" ).Valid( Widget == widget );
            using (Lock.Enter()) {
                Widget.Detach( this );
                Widget = null;
            }
            if (widget.DisposeAutomatically) {
                widget.Dispose();
            }
        }

        // ShowWidget
        public virtual void ShowWidget(UIWidgetBase widget) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be attached" ).Valid( widget.IsAttached || widget.IsAttaching );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be valid" ).Valid( widget.Screen == this );
            if (widget.IsViewable) {
                var shadowed = Widget!.DescendantsAndSelf.TakeWhile( i => i != widget ).Where( i => i.IsViewable ).Select( i => i.View! ).ToArray();
                View.ShowWidget( widget.View, shadowed );
            }
        }
        public virtual void HideWidget(UIWidgetBase widget) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be attached" ).Valid( widget.IsAttached || widget.IsDetaching );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be valid" ).Valid( widget.Screen == this );
            if (widget.IsViewable) {
                var unshadowed = Widget!.DescendantsAndSelf.TakeWhile( i => i != widget ).Where( i => i.IsViewable ).Select( i => i.View! ).ToArray();
                View.HideWidget( widget.View, unshadowed );
            }
        }

        // OnDescendantWidgetAttach
        public virtual void OnBeforeDescendantWidgetAttach(UIWidgetBase descendant) {
        }
        public virtual void OnAfterDescendantWidgetAttach(UIWidgetBase descendant) {
        }
        public virtual void OnBeforeDescendantWidgetDetach(UIWidgetBase descendant) {
        }
        public virtual void OnAfterDescendantWidgetDetach(UIWidgetBase descendant) {
        }

        // Helpers/AddScreen
        protected static void AddScreen(UIDocument document, VisualElement screen) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document != null );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen != null );
            Assert.Object.Message( $"Document {document} must be awakened" ).Valid( document.didAwake );
            Assert.Object.Message( $"Document {document} must be alive" ).Alive( document != null );
            document.rootVisualElement.Add( screen );
        }
        protected static void AddScreenIfNeeded(UIDocument document, VisualElement screen) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document != null );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen != null );
            Assert.Object.Message( $"Document {document} must be awakened" ).Valid( document.didAwake );
            Assert.Object.Message( $"Document {document} must be alive" ).Alive( document != null );
            if (!document.rootVisualElement.Contains( screen )) {
                document.rootVisualElement.Add( screen );
            }
        }
        protected static void RemoveScreen(UIDocument document, VisualElement screen) {
            Assert.Argument.Message( $"Argument 'document' must be non-null" ).NotNull( document != null );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen != null );
            Assert.Object.Message( $"Document {document} must be awakened" ).Valid( document.didAwake );
            Assert.Object.Message( $"Document {document} must be alive" ).Alive( document != null );
            document.rootVisualElement.Remove( screen );
        }

    }
    public abstract class UIScreenBase<TView> : UIScreenBase where TView : notnull, UIScreenViewBase {

        // View
        public new TView View {
            get => (TView) base.View;
            protected set => base.View = value;
        }

        // Awake
        public new void Awake() {
            base.Awake();
        }
        public new void OnDestroy() {
            base.OnDestroy();
        }

    }
}

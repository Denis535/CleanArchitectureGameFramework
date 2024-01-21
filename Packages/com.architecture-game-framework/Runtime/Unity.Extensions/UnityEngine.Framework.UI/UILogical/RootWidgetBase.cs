#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;
 
    public abstract class RootWidgetBase<TView> : UIWidgetBase<TView> where TView : RootWidgetViewBase {

        // Constructor
        public RootWidgetBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach() {
        }
        public override void OnDetach() {
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            if (descendant.IsViewable && descendant.View.VisualElement.panel == null) {
                ShowWidget( descendant );
            }
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            if (descendant.IsViewable && descendant.View.VisualElement.panel != null) {
                HideWidget( descendant );
            }
            base.OnAfterDescendantDetach( descendant );
        }

        // ShowWidget
        protected virtual void ShowWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                View.WidgetSlot.Add( widget.View! );
                OnShowWidget( widget );
            } else {
                View.ModalWidgetSlot.Add( widget.View! );
                OnShowWidget( widget );
            }
        }
        protected virtual void HideWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                Assert.Operation.Message( $"You can hide only last widget in widget slot" ).Valid( View.WidgetSlot.Children.LastOrDefault() == widget.View!.VisualElement );
                OnHideWidget( widget );
                View.WidgetSlot.Remove( widget.View! );
            } else {
                Assert.Operation.Message( $"You can hide only last widget in modal widget slot" ).Valid( View.ModalWidgetSlot.Children.LastOrDefault() == widget.View!.VisualElement );
                OnHideWidget( widget );
                View.ModalWidgetSlot.Remove( widget.View! );
            }
        }

        // OnShowWidget
        protected virtual void OnShowWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                var covered = View.WidgetSlot.Children.SkipLast( 1 ).LastOrDefault();
                covered?.SetDisplayed( false );
            } else {
                View.WidgetSlot.IsEnabled = false;
                var covered = View.ModalWidgetSlot.Children.SkipLast( 1 ).LastOrDefault();
                covered?.SetDisplayed( false );
            }
        }
        protected virtual void OnHideWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                var uncovered = View.WidgetSlot.Children.SkipLast( 1 ).LastOrDefault();
                uncovered?.SetDisplayed( true );
            } else {
                View.WidgetSlot.IsEnabled = !View.ModalWidgetSlot.Children.Any( i => i != widget.View!.VisualElement );
                var uncovered = View.ModalWidgetSlot.Children.SkipLast( 1 ).LastOrDefault();
                uncovered?.SetDisplayed( true );
            }
        }

        // Helpers/SetFocus
        protected static void SetFocus(UIWidgetBase widget) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.View!.VisualElement.panel != null );
            if (widget.View!.VisualElement.focusable) {
                widget.View!.VisualElement.Focus();
            } else {
                widget.View!.VisualElement.focusable = true;
                widget.View!.VisualElement.delegatesFocus = true;
                widget.View!.VisualElement.Focus();
                widget.View!.VisualElement.delegatesFocus = false;
                widget.View!.VisualElement.focusable = false;
            }
        }
        protected static void LoadFocus(UIWidgetBase widget) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.View!.VisualElement.panel != null );
            var focusedElement = (VisualElement?) widget.View!.VisualElement.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
            }
        }
        protected static void SaveFocus(UIWidgetBase widget) {
            SaveFocus( widget, widget.View!.VisualElement.focusController.focusedElement );
        }
        protected static void SaveFocus(UIWidgetBase widget, Focusable focusedElement) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.View!.VisualElement.panel != null );
            if (focusedElement != null && (widget.View!.VisualElement == focusedElement || widget.View!.VisualElement.Contains( (VisualElement) focusedElement ))) {
                widget.View!.VisualElement.userData = focusedElement;
            } else {
                widget.View!.VisualElement.userData = null;
            }
        }

    }
    public class RootWidget : RootWidgetBase<RootWidgetView> {

        // View
        public override RootWidgetView View { get; }

        // Constructor
        public RootWidget() {
            View = new RootWidgetView();
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

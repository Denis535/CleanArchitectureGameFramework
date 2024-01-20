#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class RootWidget : UIWidgetBase<RootWidgetView> {

        // View
        public override RootWidgetView View { get; }

        // Constructor
        public RootWidget() {
            View = new RootWidgetView();
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
        public virtual void ShowWidget(UIWidgetBase widget) {
            ShowWidgetView( widget.View!.VisualElement, widget.IsModal() );
        }
        public virtual void HideWidget(UIWidgetBase widget) {
            HideWidgetView( widget.View!.VisualElement, widget.IsModal() );
        }

        // ShowWidgetView
        public virtual void ShowWidgetView(VisualElement view, bool isModal) {
            if (!isModal) {
                View.WidgetSlot.Children.LastOrDefault()?.SetDisplayed( false );
                View.WidgetSlot.Add( view );
            } else {
                View.WidgetSlot.Children.LastOrDefault()?.SetEnabled( false );
                View.ModalWidgetSlot.Children.LastOrDefault()?.SetDisplayed( false );
                View.ModalWidgetSlot.Add( view );
            }
        }
        public virtual void HideWidgetView(VisualElement view, bool isModal) {
            if (!isModal) {
                View.WidgetSlot.Remove( view );
                View.WidgetSlot.Children.LastOrDefault()?.SetDisplayed( true );
            } else {
                View.ModalWidgetSlot.Remove( view );
                View.ModalWidgetSlot.Children.LastOrDefault()?.SetDisplayed( true );
                View.WidgetSlot.Children.LastOrDefault()?.SetEnabled( !View.ModalWidgetSlot.Children.Any() );
            }
        }

        // Helpers/SetFocus
        protected static void SetFocus(VisualElement view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.panel != null );
            if (view.focusable) {
                view.Focus();
            } else {
                view.focusable = true;
                view.delegatesFocus = true;
                view.Focus();
                view.delegatesFocus = false;
                view.focusable = false;
            }
        }
        protected static void LoadFocus(VisualElement view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.panel != null );
            var focusedElement = (VisualElement?) view.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
            }
        }
        protected static void SaveFocus(VisualElement view) {
            SaveFocus( view, view.focusController.focusedElement );
        }
        protected static void SaveFocus(VisualElement view, Focusable focusedElement) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.panel != null );
            if (focusedElement != null && (view == focusedElement || view.Contains( (VisualElement) focusedElement ))) {
                view.userData = focusedElement;
            } else {
                view.userData = null;
            }
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
                // show view
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
                // hide view
            }
            base.OnAfterDescendantDetach( descendant );
        }

        // Helpers/AddWidget
        protected static void AddWidget(VisualElement container, VisualElement widget, VisualElement? shadowed) {
            if (shadowed != null) {
                shadowed.style.display = DisplayStyle.None;
                shadowed.SetEnabled( false );
            }
            container.Add( widget );
        }
        protected static void RemoveWidget(VisualElement container, VisualElement widget, VisualElement? unshadowed) {
            container.Remove( widget );
            if (unshadowed != null) {
                unshadowed.SetEnabled( true );
                unshadowed.style.display = DisplayStyle.Flex;
            }
        }
        // Helpers/AddModalWidget
        protected static void AddModalWidget(VisualElement container, VisualElement widget, VisualElement? shadowed) {
            shadowed?.SetEnabled( false );
            container.Add( widget );
        }
        protected static void RemoveModalWidget(VisualElement container, VisualElement widget, VisualElement? unshadowed) {
            container.Remove( widget );
            unshadowed?.SetEnabled( true );
        }
        // Helpers/SetFocus
        protected static void SetFocus(VisualElement widget) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.panel != null );
            if (widget.focusable) {
                widget.Focus();
            } else {
                widget.focusable = true;
                widget.delegatesFocus = true;
                widget.Focus();
                widget.delegatesFocus = false;
                widget.focusable = false;
            }
        }
        protected static void LoadFocus(VisualElement widget) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.panel != null );
            var focusedElement = (VisualElement?) widget.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
            }
        }
        protected static void SaveFocus(VisualElement widget) {
            SaveFocus( widget, widget.focusController.focusedElement );
        }
        protected static void SaveFocus(VisualElement widget, Focusable focusedElement) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.panel != null );
            if (focusedElement != null && (widget == focusedElement || widget.Contains( (VisualElement) focusedElement ))) {
                widget.userData = focusedElement;
            } else {
                widget.userData = null;
            }
        }

    }
}

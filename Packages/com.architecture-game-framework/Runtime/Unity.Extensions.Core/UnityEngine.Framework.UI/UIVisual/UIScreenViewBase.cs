#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenViewBase : UIViewBase {

        // Constructor
        public UIScreenViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // ShowWidget
        public abstract void ShowWidget(UIWidgetViewBase widget, UIWidgetViewBase[] shadowed);
        public abstract void HideWidget(UIWidgetViewBase widget, UIWidgetViewBase[] unshadowed);

        // Helpers/CreateScreen
        protected static VisualElement CreateScreen(out VisualElement screen, out VisualElement container, out VisualElement modalContainer) {
            screen = new VisualElement();
            screen.name = "screen-view";
            screen.AddToClassList( "screen-view" );
            screen.pickingMode = PickingMode.Ignore;
            {
                container = new VisualElement();
                container.name = "views-container";
                container.AddToClassList( "views-container" );
                container.pickingMode = PickingMode.Ignore;
                screen.Add( container );
            }
            {
                modalContainer = new VisualElement();
                modalContainer.name = "modal-views-container";
                modalContainer.AddToClassList( "modal-views-container" );
                modalContainer.pickingMode = PickingMode.Ignore;
                screen.Add( modalContainer );
            }
            return screen;
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

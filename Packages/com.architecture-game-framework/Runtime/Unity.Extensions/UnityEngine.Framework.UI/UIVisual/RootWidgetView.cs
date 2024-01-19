#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class RootWidgetView : UIViewBase {

        // VisualElement
        public override VisualElement VisualElement { get; }
        public ElementWrapper Widget { get; }
        public SlotWrapper WidgetSlot { get; }
        public SlotWrapper ModalWidgetSlot { get; }

        // Constructor
        public RootWidgetView() {
            VisualElement = CreateVisualElement( out var widget, out var widgetSlot, out var modalWidgetSlot );
            Widget = widget.Wrap();
            WidgetSlot = widgetSlot.AsSlot();
            ModalWidgetSlot = modalWidgetSlot.AsSlot();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddWidget
        public virtual void AddWidget(UIViewBase widget) {
            if (!widget.IsModal()) {
                WidgetSlot.Add( widget );
            } else {
                ModalWidgetSlot.Add( widget );
            }
        }
        public virtual void RemoveWidget(UIViewBase widget) {
            if (!widget.IsModal()) {
                WidgetSlot.Remove( widget );
            } else {
                ModalWidgetSlot.Remove( widget );
            }
        }

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement widget, out VisualElement widgetSlot, out VisualElement modalWidgetSlot) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            {
                widgetSlot = new VisualElement();
                widgetSlot.name = "widget-slot";
                widgetSlot.AddToClassList( "widget-slot" );
                widgetSlot.pickingMode = PickingMode.Ignore;
                widget.Add( widgetSlot );
            }
            {
                modalWidgetSlot = new VisualElement();
                modalWidgetSlot.name = "modal-widget-slot";
                modalWidgetSlot.AddToClassList( "modal-widget-slot" );
                modalWidgetSlot.pickingMode = PickingMode.Ignore;
                widget.Add( modalWidgetSlot );
            }
            return widget;
        }
        // Helpers/AddWidget
        protected static void AddWidget(SlotWrapper slot, VisualElement widget, VisualElement? shadowed) {
            if (shadowed != null) {
                shadowed.SetEnabled( false );
                shadowed.SetDisplayed( false );
            }
            slot.Add( widget );
        }
        protected static void RemoveWidget(SlotWrapper slot, VisualElement widget, VisualElement? unshadowed) {
            slot.Remove( widget );
            if (unshadowed != null) {
                unshadowed.SetDisplayed( true );
                unshadowed.SetEnabled( true );
            }
        }
        // Helpers/AddModalWidget
        protected static void AddModalWidget(SlotWrapper slot, VisualElement widget, VisualElement? shadowed) {
            if (shadowed != null) {
                shadowed.SetEnabled( false );
            }
            slot.Add( widget );
        }
        protected static void RemoveModalWidget(SlotWrapper slot, VisualElement widget, VisualElement? unshadowed) {
            slot.Remove( widget );
            if (unshadowed != null) {
                unshadowed.SetEnabled( true );
            }
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

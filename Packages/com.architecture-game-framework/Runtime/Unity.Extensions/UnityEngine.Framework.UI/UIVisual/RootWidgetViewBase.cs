#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class RootWidgetViewBase : UIViewBase {

        // VisualElement
        public abstract ElementWrapper Widget { get; }
        public abstract SlotWrapper WidgetSlot { get; }
        public abstract SlotWrapper ModalWidgetSlot { get; }

        // Constructor
        public RootWidgetViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public class RootWidgetView : RootWidgetViewBase {

        // VisualElement
        public override VisualElement VisualElement { get; }
        public override ElementWrapper Widget { get; }
        public override SlotWrapper WidgetSlot { get; }
        public override SlotWrapper ModalWidgetSlot { get; }

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

    }
}

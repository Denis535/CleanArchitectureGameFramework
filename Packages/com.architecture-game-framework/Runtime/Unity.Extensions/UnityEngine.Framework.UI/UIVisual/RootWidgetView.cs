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
        public SlotWrapper WidgetContainer { get; }
        public SlotWrapper ModalWidgetContainer { get; }

        // Constructor
        public RootWidgetView() {
            VisualElement = CreateVisualElement( out var widget, out var widgetContainer, out var modalWidgetContainer );
            Widget = widget.Wrap();
            WidgetContainer = widgetContainer.AsSlot();
            ModalWidgetContainer = modalWidgetContainer.AsSlot();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        private static VisualElement CreateVisualElement(out VisualElement widget, out VisualElement widgetContainer, out VisualElement modalWidgetContainer) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            {
                widgetContainer = new VisualElement();
                widgetContainer.name = "widget-container";
                widgetContainer.AddToClassList( "widget-container" );
                widgetContainer.pickingMode = PickingMode.Ignore;
                widget.Add( widgetContainer );
            }
            {
                modalWidgetContainer = new VisualElement();
                modalWidgetContainer.name = "modal-widget-container";
                modalWidgetContainer.AddToClassList( "modal-widget-container" );
                modalWidgetContainer.pickingMode = PickingMode.Ignore;
                widget.Add( modalWidgetContainer );
            }
            return widget;
        }

    }
}

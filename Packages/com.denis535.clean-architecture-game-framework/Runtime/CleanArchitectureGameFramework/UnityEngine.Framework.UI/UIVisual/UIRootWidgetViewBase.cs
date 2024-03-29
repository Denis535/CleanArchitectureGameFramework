#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        // VisualElement
        public abstract ElementWrapper Widget { get; }
        public abstract WidgetListSlotWrapper<UIWidgetBase> WidgetSlot { get; }
        public abstract WidgetListSlotWrapper<UIWidgetBase> ModalWidgetSlot { get; }

        // Constructor
        public UIRootWidgetViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public class UIRootWidgetView : UIRootWidgetViewBase {

        // View
        public override ElementWrapper Widget { get; }
        public override WidgetListSlotWrapper<UIWidgetBase> WidgetSlot { get; }
        public override WidgetListSlotWrapper<UIWidgetBase> ModalWidgetSlot { get; }

        // Constructor
        public UIRootWidgetView() {
            VisualElement = CreateVisualElement( out var widget, out var widgetList, out var modalWidgetList );
            Widget = widget.Wrap();
            WidgetSlot = widgetList.AsWidgetListSlot<UIWidgetBase>();
            ModalWidgetSlot = modalWidgetList.AsWidgetListSlot<UIWidgetBase>();
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

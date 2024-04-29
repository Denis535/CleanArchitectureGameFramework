#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        // Root
        public abstract ElementWrapper Root { get; }
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

        // Root
        public override ElementWrapper Root { get; }
        public override WidgetListSlotWrapper<UIWidgetBase> WidgetSlot { get; }
        public override WidgetListSlotWrapper<UIWidgetBase> ModalWidgetSlot { get; }

        // Constructor
        public UIRootWidgetView() {
            VisualElement = CreateVisualElement( out var root, out var widgetSlot, out var modalWidgetSlot );
            Root = root.Wrap();
            WidgetSlot = widgetSlot.AsWidgetListSlot<UIWidgetBase>();
            ModalWidgetSlot = modalWidgetSlot.AsWidgetListSlot<UIWidgetBase>();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement root, out VisualElement widgetSlot, out VisualElement modalWidgetSlot) {
            root = new VisualElement();
            root.name = "root-widget";
            root.AddToClassList( "root-widget" );
            root.pickingMode = PickingMode.Ignore;
            {
                widgetSlot = new VisualElement();
                widgetSlot.name = "widget-slot";
                widgetSlot.AddToClassList( "widget-slot" );
                widgetSlot.pickingMode = PickingMode.Ignore;
                root.Add( widgetSlot );
            }
            {
                modalWidgetSlot = new VisualElement();
                modalWidgetSlot.name = "modal-widget-slot";
                modalWidgetSlot.AddToClassList( "modal-widget-slot" );
                modalWidgetSlot.pickingMode = PickingMode.Ignore;
                root.Add( modalWidgetSlot );
            }
            return root;
        }

    }
}

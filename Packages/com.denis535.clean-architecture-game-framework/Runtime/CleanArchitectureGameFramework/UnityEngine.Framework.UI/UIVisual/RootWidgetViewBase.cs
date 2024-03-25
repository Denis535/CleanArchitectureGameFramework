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
        public abstract ViewListWrapper<UIViewBase> WidgetList { get; }
        public abstract ViewListWrapper<UIViewBase> ModalWidgetList { get; }

        // Constructor
        public RootWidgetViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public class RootWidgetView : RootWidgetViewBase {

        // VisualElement
        protected internal override VisualElement VisualElement { get; }
        public override ElementWrapper Widget { get; }
        public override ViewListWrapper<UIViewBase> WidgetList { get; }
        public override ViewListWrapper<UIViewBase> ModalWidgetList { get; }

        // Constructor
        public RootWidgetView() {
            VisualElement = CreateVisualElement( out var widget, out var widgetSlot, out var modalWidgetSlot );
            Widget = widget.Wrap();
            WidgetList = widgetSlot.AsViewList<UIViewBase>();
            ModalWidgetList = modalWidgetSlot.AsViewList<UIViewBase>();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement widget, out VisualElement widgetList, out VisualElement modalWidgetList) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            {
                widgetList = new VisualElement();
                widgetList.name = "widget-list";
                widgetList.AddToClassList( "widget-list" );
                widgetList.pickingMode = PickingMode.Ignore;
                widget.Add( widgetList );
            }
            {
                modalWidgetList = new VisualElement();
                modalWidgetList.name = "modal-widget-list";
                modalWidgetList.AddToClassList( "modal-widget-list" );
                modalWidgetList.pickingMode = PickingMode.Ignore;
                widget.Add( modalWidgetList );
            }
            return widget;
        }

    }
}

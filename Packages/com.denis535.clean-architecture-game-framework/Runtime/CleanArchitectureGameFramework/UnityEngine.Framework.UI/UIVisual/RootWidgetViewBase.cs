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
        public abstract WidgetListWrapper<UIWidgetBase> WidgetList { get; }
        public abstract WidgetListWrapper<UIWidgetBase> ModalWidgetList { get; }

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
        public override WidgetListWrapper<UIWidgetBase> WidgetList { get; }
        public override WidgetListWrapper<UIWidgetBase> ModalWidgetList { get; }

        // Constructor
        public RootWidgetView() {
            VisualElement = CreateVisualElement( out var widget, out var widgetList, out var modalWidgetList );
            Widget = widget.Wrap();
            WidgetList = widgetList.AsWidgetList<UIWidgetBase>();
            ModalWidgetList = modalWidgetList.AsWidgetList<UIWidgetBase>();
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

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        protected readonly VisualElement widget;

        // Children
        public IEnumerable<UIViewBase> Children => widget.Children().Select( i => i.GetView() );

        // Constructor
        public UIRootWidgetViewBase() {
            VisualElement = CreateVisualElement( out widget );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // CreateVisualElement
        protected virtual VisualElement CreateVisualElement(out VisualElement widget) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "widget" );
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            return widget;
        }

        // AddView
        public void AddView(UIViewBase view) {
            widget.Add( view );
            widget.Sort( (a, b) => Comparer<int>.Default.Compare( a.GetView().Priority, b.GetView().Priority ) );
        }
        public void RemoveView(UIViewBase view) {
            widget.Remove( view );
        }

        // OnEvent
        public void OnSubmit(EventCallback<NavigationSubmitEvent> callback) {
            widget.OnSubmit( callback, TrickleDown.TrickleDown );
        }
        public void OnCancel(EventCallback<NavigationCancelEvent> callback) {
            widget.OnCancel( callback, TrickleDown.TrickleDown );
        }

    }
}

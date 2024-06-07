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
        public virtual void AddView(UIViewBase view) {
            widget.Add( view );
            widget.Sort( (a, b) => Comparer<int>.Default.Compare( a.GetView().Priority, b.GetView().Priority ) );
        }
        public virtual void RemoveView(UIViewBase view) {
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
    public class UIRootWidgetView : UIRootWidgetViewBase {

        // Priority
        public override int Priority => 0;
        // IsAlwaysVisible
        public override bool IsAlwaysVisible => false;
        // IsModal
        public override bool IsModal => false;

        // Constructor
        public UIRootWidgetView() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public override void AddView(UIViewBase view) {
            base.AddView( view );
            Recalculate( Children.ToArray() );
        }
        public override void RemoveView(UIViewBase view) {
            base.RemoveView( view );
            Recalculate( Children.ToArray() );
        }

        // Recalculate
        protected virtual void Recalculate(UIViewBase[] views) {
            for (var i = 0; i < views.Length; i++) {
                var view = views[ i ];
                var next = views.ElementAtOrDefault( i + 1 );
                if (next == null) {
                    ShowView( view );
                } else {
                    HideView( view, next );
                }
            }
        }
        protected virtual void ShowView(UIViewBase view) {
            if (!view.IsAlwaysVisible) {
                view.VisualElement.SetEnabled( true );
                view.VisualElement.SetDisplayed( true );
            }
            if (!view.HasFocusedElement()) {
                if (!view.LoadFocus()) {
                    view.Focus();
                }
            }
        }
        protected virtual void HideView(UIViewBase view, UIViewBase next) {
            if (view.HasFocusedElement()) {
                view.SaveFocus();
            }
            if (!view.IsAlwaysVisible) {
                if (!view.IsModal && next.IsModal) {
                    view.VisualElement.SetEnabled( false );
                } else {
                    view.VisualElement.SetDisplayed( false );
                }
            }
        }

    }
}

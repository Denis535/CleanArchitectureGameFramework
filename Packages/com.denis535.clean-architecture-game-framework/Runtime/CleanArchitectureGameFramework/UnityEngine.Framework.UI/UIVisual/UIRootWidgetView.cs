#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIRootWidgetView : UIViewBase {

        protected readonly VisualElement widget;

        // Priority
        public override int Priority => 0;
        // IsAlwaysVisible
        public override bool IsAlwaysVisible => false;
        // IsModal
        public override bool IsModal => false;
        // Views
        public IEnumerable<UIViewBase> Views => widget.Children().Select( i => i.GetView() );

        // Constructor
        public UIRootWidgetView() {
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
            Recalculate( Views.ToArray() );
        }
        public virtual void RemoveView(UIViewBase view) {
            widget.Remove( view );
            Recalculate( Views.ToArray() );
        }

        // OnEvent
        public void OnSubmit(EventCallback<NavigationSubmitEvent> callback) {
            widget.OnSubmit( callback, TrickleDown.TrickleDown );
        }
        public void OnCancel(EventCallback<NavigationCancelEvent> callback) {
            widget.OnCancel( callback, TrickleDown.TrickleDown );
        }

        // Recalculate
        protected virtual void Recalculate(UIViewBase[] views) {
            for (var i = 0; i < views.Length; i++) {
                var view = views[ i ];
                var next = views.ElementAtOrDefault( i + 1 );
                if (next == null) {
                    Show( view );
                } else {
                    Hide( view, next );
                }
            }
        }
        protected virtual void Show(UIViewBase view) {
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
        protected virtual void Hide(UIViewBase view, UIViewBase next) {
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

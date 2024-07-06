#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIRootWidgetView : UIViewBase2 {

        protected readonly VisualElement widget;

        // Views
        public IEnumerable<UIViewBase> Views => widget.GetViews();
        // OnSubmit
        public event EventCallback<NavigationSubmitEvent> OnSubmitEvent {
            add => widget.RegisterCallback( value, TrickleDown.TrickleDown );
            remove => widget.UnregisterCallback( value, TrickleDown.TrickleDown );
        }
        public event EventCallback<NavigationCancelEvent> OnCancelEvent {
            add => widget.RegisterCallback( value, TrickleDown.TrickleDown );
            remove => widget.UnregisterCallback( value, TrickleDown.TrickleDown );
        }

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
        public virtual void AddView(UIViewBase2 view) {
            widget.AddView( view );
            Sort( widget );
            Recalculate( widget );
        }
        public virtual void RemoveView(UIViewBase2 view) {
            widget.RemoveView( view );
            Recalculate( widget );
        }

        // Sort
        protected virtual void Sort(VisualElement widget) {
            widget.Sort( Compare );
            int Compare(VisualElement x, VisualElement y) {
                return Comparer<int>.Default.Compare( GetPriority( x.GetView() ), GetPriority( y.GetView() ) );
            }
        }

        // Recalculate
        protected virtual void Recalculate(VisualElement widget) {
            Recalculate( widget.GetViews().ToArray() );
        }
        protected virtual void Recalculate(UIViewBase2[] views) {
            foreach (var view in views.SkipLast( 1 )) {
                if (view.HasFocusedElement()) {
                    view.SaveFocus();
                }
            }
            for (var i = 0; i < views.Length; i++) {
                var view = views[ i ];
                var next = views.ElementAtOrDefault( i + 1 );
                if (next != null) {
                    Recalculate( view, next );
                } else {
                    Recalculate( view );
                }
            }
            if (views.Any()) {
                var view = views.Last();
                if (!view.HasFocusedElement()) {
                    if (!view.LoadFocus()) {
                        view.Focus();
                    }
                }
            }
        }
        protected virtual void Recalculate(UIViewBase2 view, UIViewBase2 next) {
            if (GetLayer( view ) == GetLayer( next )) {
                view.VisualElement.SetEnabled( false );
                view.VisualElement.SetDisplayed( false );
            } else {
                view.VisualElement.SetEnabled( false );
                view.VisualElement.SetDisplayed( true );
            }
        }
        protected virtual void Recalculate(UIViewBase2 view) {
            view.VisualElement.SetEnabled( true );
            view.VisualElement.SetDisplayed( true );
        }

        // GetPriority
        protected virtual int GetPriority(UIViewBase view) {
            return 0;
        }

        // GetLayer
        protected virtual int GetLayer(UIViewBase view) {
            return 0;
        }

    }
}

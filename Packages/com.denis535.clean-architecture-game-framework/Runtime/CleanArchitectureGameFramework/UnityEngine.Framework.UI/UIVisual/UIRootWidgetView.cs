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

        // Views
        public IEnumerable<UIViewBase> Views => widget.Children().Select( i => i.GetView() );
        // OnSubmit
        public event EventCallback<NavigationSubmitEvent> OnSubmitEvent {
            add => widget.RegisterCallback( value );
            remove => widget.UnregisterCallback( value );
        }
        public event EventCallback<NavigationCancelEvent> OnCancelEvent {
            add => widget.RegisterCallback( value );
            remove => widget.UnregisterCallback( value );
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
        public virtual void AddView(UIViewBase view) {
            widget.Add( view );
            Sort( widget );
            Recalculate( widget );
        }
        public virtual void RemoveView(UIViewBase view) {
            widget.Remove( view );
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
            Recalculate( widget.Children().Select( i => i.GetView() ).ToArray() );
        }
        protected virtual void Recalculate(UIViewBase[] views) {
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
        protected virtual void Recalculate(UIViewBase view, UIViewBase next) {
            if (GetPriority( view ) == GetPriority( next )) {
                view.VisualElement.SetEnabled( false );
                view.VisualElement.SetDisplayed( false );
            } else {
                view.VisualElement.SetEnabled( false );
                view.VisualElement.SetDisplayed( true );
            }
        }
        protected virtual void Recalculate(UIViewBase view) {
            view.VisualElement.SetEnabled( true );
            view.VisualElement.SetDisplayed( true );
        }

        // GetPriority
        protected virtual int GetPriority(UIViewBase view) {
            return 0;
        }

    }
}

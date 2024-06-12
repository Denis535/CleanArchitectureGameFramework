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

        // Layer
        public override int Layer => throw Exceptions.Internal.NotImplemented( $"Property 'Layer' is not implemented" );
        // Views
        public IEnumerable<UIViewBase> Views => widget.Children().Select( i => i.GetView() );
        // OnSubmit
        public UIObservable<NavigationSubmitEvent> OnSubmit => widget.AsObservable<NavigationSubmitEvent>();
        public UIObservable<NavigationCancelEvent> OnCancel => widget.AsObservable<NavigationCancelEvent>();

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
            widget.Sort( Compare );
            Recalculate( Views.ToArray() );
        }
        public virtual void RemoveView(UIViewBase view) {
            widget.Remove( view );
            Recalculate( Views.ToArray() );
        }

        // Recalculate
        protected virtual void Recalculate(UIViewBase[] views) {
            foreach (var view in views.SkipLast( 1 )) {
                if (view.HasFocusedElement()) {
                    view.SaveFocus();
                }
            }
            for (var i = 0; i < views.Length; i++) {
                var view = views[ i ];
                var next = views.ElementAtOrDefault( i + 1 );
                if (next == null) {
                    view.VisualElement.SetEnabled( true );
                    view.VisualElement.SetDisplayed( true );
                } else {
                    if (view.Layer == next.Layer) {
                        view.VisualElement.SetEnabled( false );
                        view.VisualElement.SetDisplayed( false );
                    } else {
                        view.VisualElement.SetEnabled( false );
                        view.VisualElement.SetDisplayed( true );
                    }
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

        // Helpers
        private static int Compare(VisualElement x, VisualElement y) {
            return Comparer<int>.Default.Compare( x.GetView().Layer, y.GetView().Layer );
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        // Views
        public IEnumerable<IUIView> Views => Children().Cast<IUIView>();
        // OnSubmit
        public event EventCallback<NavigationSubmitEvent> OnSubmitEvent {
            add => RegisterCallback( value, TrickleDown.TrickleDown );
            remove => UnregisterCallback( value, TrickleDown.TrickleDown );
        }
        public event EventCallback<NavigationCancelEvent> OnCancelEvent {
            add => RegisterCallback( value, TrickleDown.TrickleDown );
            remove => UnregisterCallback( value, TrickleDown.TrickleDown );
        }

        // Constructor
        public UIRootWidgetViewBase() : base(
            "root-widget-view",
            "widget", "widget-view",
            "root-widget", "root-widget-view" ) {
            pickingMode = PickingMode.Ignore;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public virtual void AddView(IUIView view) {
            Add( (VisualElement) view );
        }
        public virtual void RemoveView(IUIView view) {
            Remove( (VisualElement) view );
        }

    }
    public class UIRootWidgetView : UIRootWidgetViewBase {

        // Constructor
        public UIRootWidgetView() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public override void AddView(IUIView view) {
            base.AddView( view );
            Sort();
            Recalculate();
        }
        public override void RemoveView(IUIView view) {
            base.RemoveView( view );
            Recalculate();
        }

        // Sort
        protected virtual void Sort() {
            Sort( (a, b) => Comparer<int>.Default.Compare( GetPriority( a ), GetPriority( b ) ) );
        }

        // Recalculate
        protected virtual void Recalculate() {
            Recalculate( Children().ToArray() );
        }
        protected virtual void Recalculate(VisualElement[] views) {
            foreach (var view in views.SkipLast( 1 )) {
                if (view.HasFocusedElement()) {
                    view.SaveFocus();
                }
            }
            for (var i = 0; i < views.Length; i++) {
                var view = views[ i ];
                var next = views.ElementAtOrDefault( i + 1 );
                Recalculate( view, next );
            }
            if (views.Any()) {
                var view = views.Last();
                if (!view.HasFocusedElement()) {
                    if (!view.LoadFocus()) {
                        view.InitFocus();
                    }
                }
            }
        }
        protected virtual void Recalculate(VisualElement view, VisualElement? next) {
            if (next != null) {
                if (GetLayer( view ) == GetLayer( next )) {
                    view.SetEnabled( false );
                    view.SetDisplayed( false );
                } else {
                    view.SetEnabled( false );
                    view.SetDisplayed( true );
                }
            } else {
                view.SetEnabled( true );
                view.SetDisplayed( true );
            }
        }

        // GetPriority
        protected virtual int GetPriority(VisualElement view) {
            return 0;
        }

        // GetLayer
        protected virtual int GetLayer(VisualElement view) {
            return 0;
        }

    }
}

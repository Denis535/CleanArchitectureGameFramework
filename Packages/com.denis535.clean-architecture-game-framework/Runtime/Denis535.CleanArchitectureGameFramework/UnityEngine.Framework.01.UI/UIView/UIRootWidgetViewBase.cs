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
        public IEnumerable<UIViewBase> Views => Children().Cast<UIViewBase>();
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
        public UIRootWidgetViewBase() : base( "root-widget", "widget", "root-widget" ) {
            pickingMode = PickingMode.Ignore;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public virtual void AddView(UIViewBase view) {
            Add( view );
        }
        public virtual void RemoveView(UIViewBase view) {
            Remove( view );
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
        public override void AddView(UIViewBase view) {
            base.AddView( view );
            Sort();
            Recalculate();
        }
        public override void RemoveView(UIViewBase view) {
            base.RemoveView( view );
            Recalculate();
        }

        // Sort
        protected virtual void Sort() {
            Sort( Compare );
            int Compare(VisualElement x, VisualElement y) {
                return Comparer<int>.Default.Compare( GetPriority( (UIViewBase) x ), GetPriority( (UIViewBase) y ) );
            }
        }

        // Recalculate
        protected virtual void Recalculate() {
            Recalculate( Children().Cast<UIViewBase>().ToArray() );
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
                Recalculate( view, next );
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
        protected virtual void Recalculate(UIViewBase view, UIViewBase? next) {
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
        protected virtual int GetPriority(UIViewBase view) {
            return 0;
        }

        // GetLayer
        protected virtual int GetLayer(UIViewBase view) {
            return 0;
        }

    }
}

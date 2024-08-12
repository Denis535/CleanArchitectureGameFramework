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
        public UIRootWidgetViewBase() : base( "root-widget-view", "root-widget-view" ) {
            pickingMode = PickingMode.Ignore;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        protected override bool AddView(UIViewBase view) {
            Add( view );
            Sort();
            Recalculate();
            return true;
        }
        protected override bool RemoveView(UIViewBase view) {
            Remove( view );
            Recalculate();
            return true;
        }

        // Sort
        protected virtual void Sort() {
            Sort( (a, b) => Comparer<int>.Default.Compare( GetPriority( (UIViewBase) a ), GetPriority( (UIViewBase) b ) ) );
        }

        // Recalculate
        protected virtual void Recalculate() {
            var views = Children().Cast<UIViewBase>().ToList();
            foreach (var view in views.SkipLast( 1 )) {
                if (view.HasFocusedElement()) {
                    view.SaveFocus();
                }
            }
            for (var i = 0; i < views.Count; i++) {
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

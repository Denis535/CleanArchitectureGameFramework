#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class RootWidgetViewBase : ViewBase {

        // Views
        public IEnumerable<ViewBase> Views => Children().Cast<ViewBase>();
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
        public RootWidgetViewBase() {
            name = "root-widget-view";
            AddToClassList( "root-widget-view" );
            pickingMode = PickingMode.Ignore;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // TryAddView
        protected internal override bool TryAddView(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Add( view );
            Sort();
            SetVisibility( (IReadOnlyList<VisualElement>) Children() );
            return true;
        }
        protected internal override bool TryRemoveView(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Remove( view );
            SetVisibility( (IReadOnlyList<VisualElement>) Children() );
            return true;
        }

        // Sort
        protected virtual void Sort() {
            Sort( (a, b) => Comparer<int>.Default.Compare( GetOrderOf( (ViewBase) a ), GetOrderOf( (ViewBase) b ) ) );
        }
        protected virtual int GetOrderOf(ViewBase view) {
            return 0;
        }

        // SetVisibility
        protected virtual void SetVisibility(IReadOnlyList<VisualElement> views) {
            SaveFocus( views );
            foreach (var view in views) {
                view.SetEnabled( true );
                view.SetDisplayed( true );
            }
            LoadFocus( views );
        }

        // Helpers
        protected static void SaveFocus(IReadOnlyList<VisualElement> views) {
            foreach (var view in views.SkipLast( 1 ).Cast<ViewBase>()) {
                if (view.HasFocusedElement()) {
                    view.SaveFocus();
                }
            }
        }
        protected static void LoadFocus(IReadOnlyList<VisualElement> views) {
            var view = (ViewBase?) views.LastOrDefault();
            if (view != null) {
                if (!view.HasFocusedElement()) {
                    if (!view.LoadFocus()) {
                        view.InitFocus();
                    }
                }
            }
        }

    }
}

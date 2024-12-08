﻿#nullable enable
namespace UnityEngine.Framework {
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
        public UIRootWidgetViewBase() {
            name = "root-widget-view";
            AddToClassList( "root-widget-view" );
            pickingMode = PickingMode.Ignore;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        protected internal override bool AddView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Add( view );
            Sort();
            SetVisibility( (IReadOnlyList<VisualElement>) Children() );
            return true;
        }
        protected internal override bool RemoveView(UIViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Remove( view );
            SetVisibility( (IReadOnlyList<VisualElement>) Children() );
            return true;
        }

        // Sort
        protected virtual void Sort() {
            Sort( (a, b) => Comparer<int>.Default.Compare( GetOrderOf( (UIViewBase) a ), GetOrderOf( (UIViewBase) b ) ) );
        }
        protected virtual int GetOrderOf(UIViewBase view) {
            return 0;
        }

        // SetVisibility
        protected virtual void SetVisibility(IReadOnlyList<VisualElement> views) {
            foreach (var view in views.SkipLast( 1 ).Cast<UIViewBase>()) {
                if (view.HasFocusedElement()) {
                    view.SaveFocus();
                }
            }
            for (var i = 0; i < views.Count; i++) {
                var view = (UIViewBase) views[ i ];
                var next = views.Skip( i + 1 ).Cast<UIViewBase>();
                SetVisibility( view, next );
            }
            if (views.Any()) {
                var view = (UIViewBase) views.Last();
                if (!view.HasFocusedElement()) {
                    if (!view.LoadFocus()) {
                        view.InitFocus();
                    }
                }
            }
        }
        protected virtual void SetVisibility(UIViewBase view, IEnumerable<UIViewBase> next) {
            //if (next.Any()) {
            //    if (view is not MainWidgetView and not GameWidgetView) {
            //        view.SetEnabled( false );
            //    } else {
            //        view.SetEnabled( true );
            //    }
            //    if (GetPriorityOf( view ) < GetPriorityOf( next.First() )) {
            //        view.SetDisplayed( false );
            //    } else {
            //        view.SetDisplayed( true );
            //    }
            //} else {
            //    view.SetEnabled( true );
            //    view.SetDisplayed( true );
            //}
        }
        //protected virtual int GetPriorityOf(UIViewBase view) {
        //    return 0;
        //}

    }
}

﻿#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        // VisualElement
        protected internal override VisualElement VisualElement => Widget;
        // Widget
        public VisualElement Widget { get; init; } = default!;
        // Views
        public IEnumerable<UIViewBase> Views => Widget.Children().ToViews();
        // OnSubmit
        public event EventCallback<NavigationSubmitEvent> OnSubmitEvent {
            add => Widget.RegisterCallback( value, TrickleDown.TrickleDown );
            remove => Widget.UnregisterCallback( value, TrickleDown.TrickleDown );
        }
        public event EventCallback<NavigationCancelEvent> OnCancelEvent {
            add => Widget.RegisterCallback( value, TrickleDown.TrickleDown );
            remove => Widget.UnregisterCallback( value, TrickleDown.TrickleDown );
        }

        // Constructor
        public UIRootWidgetViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public virtual void AddView(UIViewBase view) {
            Widget.Add( view );
            Sort( Widget );
            Recalculate( Widget );
        }
        public virtual void RemoveView(UIViewBase view) {
            Widget.Remove( view );
            Recalculate( Widget );
        }

        // Sort
        protected virtual void Sort(VisualElement widget) {
            widget.Sort( Compare );
            int Compare(VisualElement x, VisualElement y) {
                return Comparer<int>.Default.Compare( GetPriority( x.ToView() ), GetPriority( y.ToView() ) );
            }
        }

        // Recalculate
        protected virtual void Recalculate(VisualElement widget) {
            Recalculate( widget.Children().ToViews().ToArray() );
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
                    view.VisualElement.SetEnabled( false );
                    view.VisualElement.SetDisplayed( false );
                } else {
                    view.VisualElement.SetEnabled( false );
                    view.VisualElement.SetDisplayed( true );
                }
            } else {
                view.VisualElement.SetEnabled( true );
                view.VisualElement.SetDisplayed( true );
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
    public class UIRootWidgetView : UIRootWidgetViewBase {

        // Constructor
        public UIRootWidgetView() {
            Widget = new VisualElement();
            Widget.name = "root-widget";
            Widget.AddToClassList( "widget" );
            Widget.AddToClassList( "root-widget" );
            Widget.pickingMode = PickingMode.Ignore;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

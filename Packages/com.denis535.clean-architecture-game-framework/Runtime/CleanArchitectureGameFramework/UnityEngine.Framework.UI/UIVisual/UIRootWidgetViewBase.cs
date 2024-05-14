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
        public abstract IEnumerable<UIViewBase> Views { get; }
        public abstract IEnumerable<UIViewBase> ModalViews { get; }

        // Constructor
        public UIRootWidgetViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public abstract void AddView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible);
        public abstract void RemoveView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible);

        // AddModalView
        public abstract void AddModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible);
        public abstract void RemoveModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible);

        // OnSubmit
        public abstract void OnSubmit(EventCallback<NavigationSubmitEvent> callback);
        public abstract void OnCancel(EventCallback<NavigationCancelEvent> callback);

    }
    public class UIRootWidgetView : UIRootWidgetViewBase {

        protected readonly VisualElement widget;
        protected readonly VisualElement views;
        protected readonly VisualElement modalViews;

        // Views
        public override IEnumerable<UIViewBase> Views => views.Children().Select( i => i.GetView() );
        public override IEnumerable<UIViewBase> ModalViews => modalViews.Children().Select( i => i.GetView() );

        // Constructor
        public UIRootWidgetView() {
            VisualElement = CreateVisualElement( out widget, out views, out modalViews );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // CreateVisualElement
        protected virtual VisualElement CreateVisualElement(out VisualElement widget, out VisualElement views, out VisualElement modalViews) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            {
                views = new VisualElement();
                views.name = "views";
                views.AddToClassList( "views" );
                views.pickingMode = PickingMode.Ignore;
                widget.Add( views );
            }
            {
                modalViews = new VisualElement();
                modalViews.name = "modal-views";
                modalViews.AddToClassList( "modal-views" );
                modalViews.pickingMode = PickingMode.Ignore;
                widget.Add( modalViews );
            }
            return widget;
        }

        // AddView
        public override void AddView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            AddView( views, view, isAlwaysVisible );
        }
        public override void RemoveView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            RemoveView( views, view, isAlwaysVisible );
        }

        // AddModalView
        public override void AddModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            if (views.enabledSelf) views.SetEnabled( false );
            AddView( modalViews, view, isAlwaysVisible );
        }
        public override void RemoveModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            RemoveView( modalViews, view, isAlwaysVisible );
            if (!views.enabledSelf && !modalViews.Children().Any()) views.SetEnabled( true );
        }

        // OnEvent
        public override void OnSubmit(EventCallback<NavigationSubmitEvent> callback) {
            widget.OnSubmit( callback, TrickleDown.TrickleDown );
        }
        public override void OnCancel(EventCallback<NavigationCancelEvent> callback) {
            widget.OnCancel( callback, TrickleDown.TrickleDown );
        }

        // Helpers
        protected static void AddView(VisualElement views, UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            var covered = views.Children().LastOrDefault();
            if (covered != null) {
                if (!isAlwaysVisible( covered.GetView() )) covered.SetDisplayed( false );
            }
            views.Add( view );
        }
        protected static void RemoveView(VisualElement views, UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            if (view.VisualElement == views.Children().LastOrDefault()) {
                views.Remove( view );
                var uncovered = views.Children().LastOrDefault();
                if (uncovered != null) {
                    if (!isAlwaysVisible( uncovered.GetView() )) uncovered.SetDisplayed( true );
                }
            } else {
                views.Remove( view );
            }
        }

    }
}

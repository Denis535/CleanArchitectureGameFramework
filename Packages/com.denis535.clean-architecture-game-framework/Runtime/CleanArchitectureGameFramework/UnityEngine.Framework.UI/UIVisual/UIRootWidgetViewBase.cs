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

        protected readonly VisualElement views;
        protected readonly VisualElement modalViews;

        // Views
        public override IEnumerable<UIViewBase> Views => views.Children().Select( i => (UIViewBase) i.userData );
        public override IEnumerable<UIViewBase> ModalViews => modalViews.Children().Select( i => (UIViewBase) i.userData );

        // Constructor
        public UIRootWidgetView() {
            VisualElement = CreateVisualElement( out views, out modalViews );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public override void AddView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            var last = views.Children().LastOrDefault();
            if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                last.SetEnabled( false );
                last.SetDisplayed( false );
            }
            views.Add( view );
        }
        public override void RemoveView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            views.Remove( view );
            var last = views.Children().LastOrDefault();
            if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                last.SetDisplayed( true );
                last.SetEnabled( true );
            }
        }

        // AddModalView
        public override void AddModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            views.SetEnabled( false );
            {
                var last = modalViews.Children().LastOrDefault();
                if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                    last.SetEnabled( false );
                    last.SetDisplayed( false );
                }
                modalViews.Add( view );
            }
        }
        public override void RemoveModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            {
                modalViews.Remove( view );
                var last = modalViews.Children().LastOrDefault();
                if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                    last.SetDisplayed( true );
                    last.SetEnabled( true );
                }
            }
            views.SetEnabled( !modalViews.Children().Any() );
        }

        // OnEvent
        public override void OnSubmit(EventCallback<NavigationSubmitEvent> callback) {
            VisualElement.OnSubmit( callback, TrickleDown.TrickleDown );
        }
        public override void OnCancel(EventCallback<NavigationCancelEvent> callback) {
            VisualElement.OnCancel( callback, TrickleDown.TrickleDown );
        }

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement views, out VisualElement modalViews) {
            var visualElement = new VisualElement();
            visualElement.name = "root-widget";
            visualElement.AddToClassList( "root-widget" );
            visualElement.pickingMode = PickingMode.Ignore;
            {
                views = new VisualElement();
                views.name = "views";
                views.AddToClassList( "views" );
                views.pickingMode = PickingMode.Ignore;
                visualElement.Add( views );
            }
            {
                modalViews = new VisualElement();
                modalViews.name = "modal-views";
                modalViews.AddToClassList( "modal-views" );
                modalViews.pickingMode = PickingMode.Ignore;
                visualElement.Add( modalViews );
            }
            return visualElement;
        }

    }
}

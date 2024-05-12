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

        protected readonly VisualElement viewSlot;
        protected readonly VisualElement modalViewSlot;

        // Views
        public override IEnumerable<UIViewBase> Views => viewSlot.Children().Select( i => (UIViewBase) i.userData );
        public override IEnumerable<UIViewBase> ModalViews => modalViewSlot.Children().Select( i => (UIViewBase) i.userData );

        // Constructor
        public UIRootWidgetView() {
            VisualElement = CreateVisualElement( out viewSlot, out modalViewSlot );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public override void AddView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            var last = viewSlot.Children().LastOrDefault();
            if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                last.SetEnabled( false );
                last.SetDisplayed( false );
            }
            viewSlot.Add( view );
        }
        public override void RemoveView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            viewSlot.Remove( view );
            var last = viewSlot.Children().LastOrDefault();
            if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                last.SetDisplayed( true );
                last.SetEnabled( true );
            }
        }

        // AddModalView
        public override void AddModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            viewSlot.SetEnabled( false );
            {
                var last = modalViewSlot.Children().LastOrDefault();
                if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                    last.SetEnabled( false );
                    last.SetDisplayed( false );
                }
                modalViewSlot.Add( view );
            }
        }
        public override void RemoveModalView(UIViewBase view, Func<UIViewBase, bool> isAlwaysVisible) {
            {
                modalViewSlot.Remove( view );
                var last = modalViewSlot.Children().LastOrDefault();
                if (last != null && !isAlwaysVisible( (UIViewBase) last.userData )) {
                    last.SetDisplayed( true );
                    last.SetEnabled( true );
                }
            }
            viewSlot.SetEnabled( !modalViewSlot.Children().Any() );
        }

        // OnEvent
        public override void OnSubmit(EventCallback<NavigationSubmitEvent> callback) {
            VisualElement.OnSubmit( callback, TrickleDown.TrickleDown );
        }
        public override void OnCancel(EventCallback<NavigationCancelEvent> callback) {
            VisualElement.OnCancel( callback, TrickleDown.TrickleDown );
        }

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement viewSlot, out VisualElement modalViewSlot) {
            var visualElement = new VisualElement();
            visualElement.name = "root-widget";
            visualElement.AddToClassList( "root-widget" );
            visualElement.pickingMode = PickingMode.Ignore;
            {
                viewSlot = new VisualElement();
                viewSlot.name = "view-slot";
                viewSlot.AddToClassList( "view-slot" );
                viewSlot.pickingMode = PickingMode.Ignore;
                visualElement.Add( viewSlot );
            }
            {
                modalViewSlot = new VisualElement();
                modalViewSlot.name = "modal-view-slot";
                modalViewSlot.AddToClassList( "modal-view-slot" );
                modalViewSlot.pickingMode = PickingMode.Ignore;
                visualElement.Add( modalViewSlot );
            }
            return visualElement;
        }

    }
}

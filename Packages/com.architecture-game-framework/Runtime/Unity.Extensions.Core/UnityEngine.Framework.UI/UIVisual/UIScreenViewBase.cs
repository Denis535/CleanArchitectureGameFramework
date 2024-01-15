#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenViewBase : UIViewBase {

        // Screen
        internal UIScreenBase Screen { get; }

        // Constructor
        public UIScreenViewBase(UIScreenBase screen) {
            Screen = screen;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // ShowView
        public abstract void ShowView(UIWidgetViewBase view, UIWidgetViewBase[] shadowed);
        public abstract void HideView(UIWidgetViewBase view, UIWidgetViewBase[] unshadowed);

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement view, out VisualElement viewsContainer, out VisualElement modalViewsContainer) {
            var visualElement = view = new VisualElement();
            visualElement.name = "screen-view";
            visualElement.pickingMode = PickingMode.Ignore;
            visualElement.AddToClassList( "screen-view" );
            {
                viewsContainer = new VisualElement();
                viewsContainer.name = "views-container";
                viewsContainer.pickingMode = PickingMode.Ignore;
                viewsContainer.AddToClassList( "views-container" );
                visualElement.Add( viewsContainer );
            }
            {
                modalViewsContainer = new VisualElement();
                modalViewsContainer.name = "modal-views-container";
                modalViewsContainer.pickingMode = PickingMode.Ignore;
                modalViewsContainer.AddToClassList( "modal-views-container" );
                visualElement.Add( modalViewsContainer );
            }
            return visualElement;
        }
        // Helpers/AddView
        protected static void AddView(VisualElement container, UIWidgetViewBase view, UIWidgetViewBase? shadowed) {
            if (shadowed != null) {
                shadowed.VisualElement.style.display = DisplayStyle.None;
                shadowed.VisualElement.SetEnabled( false );
            }
            container.Add( view.VisualElement );
        }
        protected static void RemoveView(VisualElement container, UIWidgetViewBase view, UIWidgetViewBase? unshadowed) {
            container.Remove( view.VisualElement );
            if (unshadowed != null) {
                unshadowed.VisualElement.SetEnabled( true );
                unshadowed.VisualElement.style.display = DisplayStyle.Flex;
            }
        }
        // Helpers/AddModalView
        protected static void AddModalView(VisualElement container, UIWidgetViewBase view, UIWidgetViewBase? shadowed) {
            shadowed?.VisualElement.SetEnabled( false );
            container.Add( view.VisualElement );
        }
        protected static void RemoveModalView(VisualElement container, UIWidgetViewBase view, UIWidgetViewBase? unshadowed) {
            container.Remove( view.VisualElement );
            unshadowed?.VisualElement.SetEnabled( true );
        }
        // Helpers/SetFocus
        protected static void SetFocus(UIWidgetViewBase view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.VisualElement.panel != null );
            if (view.VisualElement.focusable) {
                view.VisualElement.Focus();
            } else {
                view.VisualElement.focusable = true;
                view.VisualElement.delegatesFocus = true;
                view.VisualElement.Focus();
                view.VisualElement.delegatesFocus = false;
                view.VisualElement.focusable = false;
            }
        }
        protected static void LoadFocus(UIWidgetViewBase view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.VisualElement.panel != null );
            var focusedElement = (VisualElement?) view.VisualElement.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
            }
        }
        protected static void SaveFocus(UIWidgetViewBase view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.VisualElement.panel != null );
            var focusController = view.VisualElement.focusController;
            var focusedElement = (VisualElement?) focusController.focusedElement;
            if (focusedElement != null && (view.VisualElement == focusedElement || view.VisualElement.Contains( focusedElement ))) {
                view.VisualElement.userData = focusedElement;
            } else {
                view.VisualElement.userData = null;
            }
        }

    }
}

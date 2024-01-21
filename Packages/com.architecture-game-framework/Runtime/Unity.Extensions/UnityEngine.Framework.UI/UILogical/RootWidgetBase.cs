#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class RootWidgetBase<TView> : UIWidgetBase<TView> where TView : RootWidgetViewBase {

        // Constructor
        public RootWidgetBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach() {
        }
        public override void OnDetach() {
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            if (descendant.IsViewable && descendant.View.VisualElement.panel == null) {
                ShowWidget( descendant );
            }
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            if (descendant.IsViewable && descendant.View.VisualElement.panel != null) {
                HideWidget( descendant );
            }
            base.OnAfterDescendantDetach( descendant );
        }

        // ShowWidget
        protected virtual void ShowWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                var covered = widget.Descendants.Where( i => i.IsViewable && !i.IsModal() ).TakeWhile( i => i != widget ).LastOrDefault();
                ShowWidget( widget, covered );
            } else {
                var covered = widget.Descendants.Where( i => i.IsViewable && i.IsModal() ).TakeWhile( i => i != widget ).LastOrDefault();
                ShowWidget( widget, covered );
            }
        }
        protected virtual void HideWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                var uncovered = widget.Descendants.Where( i => i.IsViewable && !i.IsModal() ).TakeWhile( i => i != widget ).LastOrDefault();
                HideWidget( widget, uncovered );
            } else {
                var uncovered = widget.Descendants.Where( i => i.IsViewable && i.IsModal() ).TakeWhile( i => i != widget ).LastOrDefault();
                HideWidget( widget, uncovered );
            }
        }

        // ShowWidget
        protected virtual void ShowWidget(UIWidgetBase widget, UIWidgetBase? covered) {
            if (!widget.IsModal()) {
                View.WidgetSlot.Add( widget.View! );
                if (covered != null) covered.View!.IsDisplayed = false;
            } else {
                View.ModalWidgetSlot.Add( widget.View! );
                View.WidgetSlot.IsEnabled = View.ModalWidgetSlot.Children.Count == 0;
                if (covered != null) covered.View!.IsDisplayed = false;
            }
        }
        protected virtual void HideWidget(UIWidgetBase widget, UIWidgetBase? uncovered) {
            if (!widget.IsModal()) {
                View.WidgetSlot.Remove( widget.View! );
                if (uncovered != null) uncovered.View!.IsDisplayed = true;
            } else {
                View.ModalWidgetSlot.Remove( widget.View! );
                View.WidgetSlot.IsEnabled = View.ModalWidgetSlot.Children.Count == 0;
                if (uncovered != null) uncovered.View!.IsDisplayed = true;
            }
        }

        // Helpers/SetFocus
        protected static void SetFocus(UIWidgetBase widget) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.View!.VisualElement.panel != null );
            if (widget.View!.VisualElement.focusable) {
                widget.View!.VisualElement.Focus();
            } else {
                widget.View!.VisualElement.focusable = true;
                widget.View!.VisualElement.delegatesFocus = true;
                widget.View!.VisualElement.Focus();
                widget.View!.VisualElement.delegatesFocus = false;
                widget.View!.VisualElement.focusable = false;
            }
        }
        protected static void LoadFocus(UIWidgetBase widget) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.View!.VisualElement.panel != null );
            var focusedElement = (VisualElement?) widget.View!.VisualElement.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
            }
        }
        protected static void SaveFocus(UIWidgetBase widget) {
            SaveFocus( widget, widget.View!.VisualElement.focusController.focusedElement );
        }
        protected static void SaveFocus(UIWidgetBase widget, Focusable focusedElement) {
            Assert.Object.Message( $"Widget {widget} must be attached" ).Valid( widget.View!.VisualElement.panel != null );
            if (focusedElement != null && (widget.View!.VisualElement == focusedElement || widget.View!.VisualElement.Contains( (VisualElement) focusedElement ))) {
                widget.View!.VisualElement.userData = focusedElement;
            } else {
                widget.View!.VisualElement.userData = null;
            }
        }

    }
    public abstract class RootWidget : RootWidgetBase<RootWidgetView> {

        // View
        public override RootWidgetView View { get; }

        // Constructor
        public RootWidget() {
            View = new RootWidgetView();
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

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
            // cover last widget
            if (!widget.IsModal()) {
                // cover last normal widget
                var last = (VisualElement?) View.WidgetSlot.Children.LastOrDefault();
                if (last != null) {
                    SaveFocus( last );
                    last.SetDisplayed( false );
                }
            } else {
                // if you have any modal widget
                // then cover last modal widget
                // otherwise cover last normal widget
                if (View.ModalWidgetSlot.Children.Any()) {
                    var last = (VisualElement?) View.ModalWidgetSlot.Children.LastOrDefault();
                    if (last != null) {
                        SaveFocus( last );
                        last.SetDisplayed( false );
                    }
                } else {
                    var last = (VisualElement?) View.WidgetSlot.Children.LastOrDefault();
                    if (last != null) {
                        SaveFocus( last );
                        last.SetEnabled( false );
                    }
                }
            }
            // show widget
            if (!widget.IsModal()) {
                View.WidgetSlot.Add( widget.View!.VisualElement );
                SetFocus( widget.View!.VisualElement );
            } else {
                View.ModalWidgetSlot.Add( widget.View!.VisualElement );
                SetFocus( widget.View!.VisualElement );
            }
        }
        protected virtual void HideWidget(UIWidgetBase widget) {
            // hide widget
            if (!widget.IsModal()) {
                Assert.Operation.Message( $"You can remove only last widget in widget slot" ).Valid( View.WidgetSlot.Children.LastOrDefault() == widget.View!.VisualElement );
                View.WidgetSlot.Remove( widget.View.VisualElement );
            } else {
                Assert.Operation.Message( $"You can remove only last widget in modal widget slot" ).Valid( View.ModalWidgetSlot.Children.LastOrDefault() == widget.View!.VisualElement );
                View.ModalWidgetSlot.Remove( widget.View.VisualElement );
            }
            // uncover last widget
            if (!widget.IsModal()) {
                // uncover last normal widget
                var last = (VisualElement?) View.WidgetSlot.Children.LastOrDefault();
                if (last != null) {
                    last.SetDisplayed( true );
                    LoadFocus( last );
                }
            } else {
                // if you have any modal widget
                // then uncover last modal widget
                // otherwise uncover last normal widget
                if (View.ModalWidgetSlot.Children.Any()) {
                    var last = (VisualElement?) View.ModalWidgetSlot.Children.LastOrDefault();
                    if (last != null) {
                        last.SetDisplayed( true );
                        LoadFocus( last );
                    }
                } else {
                    var last = (VisualElement?) View.WidgetSlot.Children.LastOrDefault();
                    if (last != null) {
                        last.SetEnabled( true );
                        LoadFocus( last );
                    }
                }
            }
        }

        // Helpers/SetFocus
        protected static void SetFocus(VisualElement view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view!.panel != null );
            if (view.focusable) {
                view.Focus();
            } else {
                view.focusable = true;
                view.delegatesFocus = true;
                view.Focus();
                view.delegatesFocus = false;
                view.focusable = false;
            }
        }
        protected static void SaveFocus(VisualElement view) {
            SaveFocus( view, view.focusController.focusedElement );
        }
        protected static void SaveFocus(VisualElement view, Focusable focusedElement) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view.panel != null );
            if (focusedElement != null && (view == focusedElement || view!.Contains( (VisualElement) focusedElement ))) {
                view!.userData = focusedElement;
            } else {
                view!.userData = null;
            }
        }
        protected static void LoadFocus(VisualElement view) {
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view!.panel != null );
            var focusedElement = (VisualElement?) view.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
            }
        }

    }
    public class RootWidget : RootWidgetBase<RootWidgetView> {

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

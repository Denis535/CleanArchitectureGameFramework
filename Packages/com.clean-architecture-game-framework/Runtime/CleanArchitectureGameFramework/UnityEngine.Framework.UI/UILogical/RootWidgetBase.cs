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

        // ShowWidget
        protected override void ShowWidget(UIWidgetBase widget) {
            base.ShowWidget( widget );
        }
        protected override void HideWidget(UIWidgetBase widget) {
            base.HideWidget( widget );
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
        protected override RootWidgetView View { get; }

        // Constructor
        public RootWidget() {
            View = new RootWidgetView();
            View.Widget.OnEventTrickleDown<NavigationSubmitEvent>( evt => {
                if (evt.target is Button button) {
                    using (var click = ClickEvent.GetPooled()) {
                        click.target = button;
                        button.SendEvent( click );
                    }
                    evt.StopPropagation();
                }
            } );
            View.Widget.OnEventTrickleDown<NavigationCancelEvent>( evt => {
                var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().FirstOrDefault( i => i.name.Contains( "widget" ) );
                var button = widget?.Query<Button>().Where( i => i.name is "resume" or "cancel" or "cancellation" or "back" or "no" or "quit" ).First();
                if (button != null) {
                    using (var click = ClickEvent.GetPooled()) {
                        click.target = button;
                        button.SendEvent( click );
                    }
                    evt.StopPropagation();
                }
            } );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // ShowWidget
        protected override void ShowWidget(UIWidgetBase widget) {
            OnBeforeShowWidget( widget );
            OnShowWidget( widget );
        }
        protected override void HideWidget(UIWidgetBase widget) {
            OnHideWidget( widget );
            OnAfterHideWidget( widget );
        }

        // OnBeforeShowWidget
        protected virtual void OnBeforeShowWidget(UIWidgetBase widget) {
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
        }
        protected virtual void OnAfterHideWidget(UIWidgetBase widget) {
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

        // OnShowWidget
        protected virtual void OnShowWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                View.WidgetSlot.Add( widget.GetVisualElement()! );
                SetFocus( widget.GetVisualElement()! );
            } else {
                View.ModalWidgetSlot.Add( widget.GetVisualElement()! );
                SetFocus( widget.GetVisualElement()! );
            }
        }
        protected virtual void OnHideWidget(UIWidgetBase widget) {
            if (!widget.IsModal()) {
                Assert.Operation.Message( $"You can remove only last widget in widget slot" ).Valid( View.WidgetSlot.Children.LastOrDefault() == widget.GetVisualElement()! );
                View.WidgetSlot.Remove( widget.GetVisualElement()! );
            } else {
                Assert.Operation.Message( $"You can remove only last widget in modal widget slot" ).Valid( View.ModalWidgetSlot.Children.LastOrDefault() == widget.GetVisualElement()! );
                View.ModalWidgetSlot.Remove( widget.GetVisualElement()! );
            }
        }

    }
}

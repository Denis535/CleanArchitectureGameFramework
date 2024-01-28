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

        // Helpers
        protected static void Focus(VisualElement view) {
            Assert.Argument.Message( $"Argument 'view' must be non-null" ).NotNull( view != null );
            Assert.Object.Message( $"View {view} must be attached" ).Valid( view!.panel != null );
            if (HasFocusedElement( view )) {
                return;
            }
            if (LoadFocusedElement( view )) {
                return;
            }
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
            SaveFocusedElement( view, GetFocusedElement( view ) );
        }
        // Helpers
        private static bool HasFocusedElement(VisualElement view) {
            var focusedElement = (VisualElement) view.focusController.focusedElement;
            if (focusedElement != null && focusedElement.GetAncestorsAndSelf().Contains( view )) return true;
            return false;
        }
        private static VisualElement? GetFocusedElement(VisualElement view) {
            var focusedElement = (VisualElement) view.focusController.focusedElement;
            if (focusedElement != null && focusedElement.GetAncestorsAndSelf().Contains( view )) return focusedElement;
            return null;
        }
        private static void SaveFocusedElement(VisualElement view, VisualElement? focusedElement) {
            view.userData = focusedElement;
        }
        private static bool LoadFocusedElement(VisualElement view) {
            var focusedElement = (VisualElement?) view.userData;
            if (focusedElement != null) {
                focusedElement.Focus();
                return true;
            }
            return false;
        }

    }
    public class RootWidget : RootWidgetBase<RootWidgetView> {

        // View
        protected override RootWidgetView View { get; }
        private List<UIWidgetBase> Widgets_ { get; } = new List<UIWidgetBase>();
        private List<UIWidgetBase> ModalWidgets_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Widgets => Widgets_;
        public IReadOnlyList<UIWidgetBase> ModalWidgets => ModalWidgets_;

        // Constructor
        public RootWidget() {
            View = CreateView();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // ShowWidget
        protected override void ShowWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                OnShowWidget( widget );
                OnAfterShowWidget();
            }
        }
        protected override void HideWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                OnHideWidget( widget );
                OnAfterHideWidget();
            }
        }

        // OnShowWidget
        protected virtual void OnShowWidget(UIWidgetBase widget) {
            if (widget.IsModal()) {
                ModalWidgets_.Add( widget );
                View.ModalWidgetSlot.Add( widget.GetVisualElement()! );
            } else {
                Widgets_.Add( widget );
                View.WidgetSlot.Add( widget.GetVisualElement()! );
            }
        }
        protected virtual void OnHideWidget(UIWidgetBase widget) {
            if (widget.IsModal()) {
                Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == ModalWidgets.LastOrDefault() );
                ModalWidgets_.Remove( widget );
                View.ModalWidgetSlot.Remove( widget.GetVisualElement()! );
            } else {
                Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == Widgets.LastOrDefault() );
                Widgets_.Remove( widget );
                View.WidgetSlot.Remove( widget.GetVisualElement()! );
            }
        }

        // OnAfterShowWidget
        protected virtual void OnAfterShowWidget() {
            foreach (var widget in Widgets) {
                widget.GetVisualElement()!.SetEnabled( !ModalWidgets.Any() );
                widget.GetVisualElement()!.SetDisplayed( widget == Widgets.LastOrDefault() );
            }
            foreach (var widget in ModalWidgets) {
                widget.GetVisualElement()!.SetDisplayed( widget == ModalWidgets.LastOrDefault() );
            }
        }
        protected virtual void OnAfterHideWidget() {
            foreach (var widget in Widgets) {
                widget.GetVisualElement()!.SetEnabled( !ModalWidgets.Any() );
                widget.GetVisualElement()!.SetDisplayed( widget == Widgets.LastOrDefault() );
            }
            foreach (var widget in ModalWidgets) {
                widget.GetVisualElement()!.SetDisplayed( widget == ModalWidgets.LastOrDefault() );
            }
        }

        // Helpers
        private static RootWidgetView CreateView() {
            var view = new RootWidgetView();
            view.Widget.OnEventTrickleDown<NavigationSubmitEvent>( evt => {
                if (evt.target is Button button) {
                    using (var click = ClickEvent.GetPooled()) {
                        click.target = button;
                        button.SendEvent( click );
                    }
                    evt.StopPropagation();
                }
            } );
            view.Widget.OnEventTrickleDown<NavigationCancelEvent>( evt => {
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
            return view;
        }

    }
}

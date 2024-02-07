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
        public override void OnAttach(object? argument) {
        }
        public override void OnDetach(object? argument) {
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
            if (focusedElement != null && (view == focusedElement || view.Contains( focusedElement ))) return true;
            return false;
        }
        private static VisualElement? GetFocusedElement(VisualElement view) {
            view.IsAncestorOf( view );
            var focusedElement = (VisualElement) view.focusController.focusedElement;
            if (focusedElement != null && (view == focusedElement || view.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        private static void SaveFocusedElement(VisualElement view, VisualElement? focusedElement) {
            view.userData = focusedElement;
        }
        private static bool LoadFocusedElement(VisualElement view) {
            var focusedElement = (VisualElement?) view.userData;
            view.userData = null;
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

        // OnAttach
        public override void OnAttach(object? argument) {
            base.OnAttach( argument );
        }
        public override void OnDetach(object? argument) {
            base.OnDetach( argument );
        }

        // ShowWidget
        protected override void ShowWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    ModalWidgets_.Add( widget );
                    View.ModalWidgetSlot.Add( widget.GetVisualElement()! );
                } else {
                    Widgets_.Add( widget );
                    View.WidgetSlot.Add( widget.GetVisualElement()! );
                }
                {
                    var covered = (UIWidgetBase?) Widgets.Concat( ModalWidgets ).SkipLast( 1 ).LastOrDefault();
                    if (covered != null) SaveFocus( covered.GetVisualElement()! );
                    RecalcVisibility();
                    var newWidget = (UIWidgetBase?) Widgets.Concat( ModalWidgets ).LastOrDefault();
                    if (newWidget != null) Focus( newWidget.GetVisualElement()! );
                }
            }
        }
        protected override void HideWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == ModalWidgets.LastOrDefault() );
                    ModalWidgets_.Remove( widget );
                    View.ModalWidgetSlot.Remove( widget.GetVisualElement()! );
                } else {
                    Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == Widgets.LastOrDefault() );
                    Widgets_.Remove( widget );
                    View.WidgetSlot.Remove( widget.GetVisualElement()! );
                }
                {
                    RecalcVisibility();
                    var uncovered = (UIWidgetBase?) Widgets.Concat( ModalWidgets ).LastOrDefault();
                    if (uncovered != null) Focus( uncovered.GetVisualElement()! );
                }
            }
        }

        // RecalcVisibility
        protected virtual void RecalcVisibility() {
            foreach (var widget in Widgets.SkipLast( 1 )) {
                // hide covered widgets
                widget.GetVisualElement()!.SetEnabled( true );
                widget.GetVisualElement()!.SetDisplayed( false );
            }
            if (Widgets.Any()) {
                // show new widget or unhide uncover widget
                var widget = Widgets.Last();
                widget.GetVisualElement()!.SetEnabled( !ModalWidgets.Any() );
                widget.GetVisualElement()!.SetDisplayed( true );
            }
            foreach (var widget in ModalWidgets.SkipLast( 1 )) {
                // hide covered widgets
                widget.GetVisualElement()!.SetEnabled( true );
                widget.GetVisualElement()!.SetDisplayed( false );
            }
            if (ModalWidgets.Any()) {
                // show new widget or unhide uncover widget
                var widget = ModalWidgets.Last();
                widget.GetVisualElement()!.SetEnabled( true );
                widget.GetVisualElement()!.SetDisplayed( true );
            }
        }

        // Helpers
        private static RootWidgetView CreateView() {
            var view = new RootWidgetView();
            view.Widget.OnEvent<NavigationSubmitEvent>( evt => {
                if (evt.target is Button button) {
                    using (var click = ClickEvent.GetPooled()) {
                        click.target = button;
                        button.SendEvent( click );
                    }
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            view.Widget.OnEvent<NavigationCancelEvent>( evt => {
                var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().FirstOrDefault( i => i.name.Contains( "widget" ) );
                var button = widget?.Query<Button>().Where( i => i.name is "resume" or "cancel" or "cancellation" or "back" or "no" or "quit" ).First();
                if (button != null) {
                    using (var click = ClickEvent.GetPooled()) {
                        click.target = button;
                        button.SendEvent( click );
                    }
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            return view;
        }

    }
}

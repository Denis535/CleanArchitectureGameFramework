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
                    var prev = (UIWidgetBase?) Widgets.Concat( ModalWidgets ).SkipLast( 1 ).LastOrDefault();
                    if (prev != null) prev.GetVisualElement()!.SaveFocus();
                    RecalcVisibility();
                    var last = (UIWidgetBase?) Widgets.Concat( ModalWidgets ).LastOrDefault();
                    if (last != null) last.GetVisualElement()!.LoadFocus();
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
                    var last = (UIWidgetBase?) Widgets.Concat( ModalWidgets ).LastOrDefault();
                    if (last != null) last.GetVisualElement()!.LoadFocus();
                }
            }
        }

        // RecalcVisibility
        protected virtual void RecalcVisibility() {
            foreach (var widget in Widgets) {
                RecalcWidgetVisibility( widget, widget == Widgets.Last() );
            }
            foreach (var widget in ModalWidgets) {
                RecalcModalWidgetVisibility( widget, widget == ModalWidgets.Last() );
            }
        }
        protected virtual void RecalcWidgetVisibility(UIWidgetBase widget, bool isLast) {
            if (!isLast) {
                // hide covered widgets
                widget.GetVisualElement()!.SetEnabled( true );
                widget.GetVisualElement()!.SetDisplayed( false );
            } else {
                // show new widget or unhide uncovered widget
                widget.GetVisualElement()!.SetEnabled( !ModalWidgets.Any() );
                widget.GetVisualElement()!.SetDisplayed( true );
            }
        }
        protected virtual void RecalcModalWidgetVisibility(UIWidgetBase widget, bool isLast) {
            if (!isLast) {
                // hide covered widgets
                widget.GetVisualElement()!.SetEnabled( true );
                widget.GetVisualElement()!.SetDisplayed( false );
            } else {
                // show new widget or unhide uncovered widget
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

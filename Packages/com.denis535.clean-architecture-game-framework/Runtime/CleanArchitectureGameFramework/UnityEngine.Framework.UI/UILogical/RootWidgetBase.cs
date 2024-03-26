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

        // OnBeforeDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            base.OnAfterDescendantDetach( descendant );
        }

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child, object? argument) {
            base.__AttachChild__( child, argument );
        }
        protected internal override void __DetachChild__(UIWidgetBase child, object? argument) {
            base.__DetachChild__( child, argument );
        }

        // ShowDescendantWidget
        protected override void ShowDescendantWidget(UIWidgetBase widget) {
            base.ShowDescendantWidget( widget );
        }
        protected override void HideDescendantWidget(UIWidgetBase widget) {
            base.HideDescendantWidget( widget );
        }

    }
    public class RootWidget : RootWidgetBase<RootWidgetViewBase> {

        // View
        protected internal override RootWidgetViewBase View { get; }
        public IReadOnlyList<UIWidgetBase> Widgets => View.WidgetSlot.Widgets;
        public IReadOnlyList<UIWidgetBase> ModalWidgets => View.ModalWidgetSlot.Widgets;

        // Constructor
        public RootWidget() {
            View = CreateView();
        }
        public RootWidget(RootWidgetViewBase view) {
            View = view;
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

        // OnBeforeDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            base.OnAfterDescendantDetach( descendant );
        }

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child, object? argument) {
            base.__AttachChild__( child, argument );
        }
        protected internal override void __DetachChild__(UIWidgetBase child, object? argument) {
            base.__DetachChild__( child, argument );
        }

        // ShowDescendantWidget
        protected override void ShowDescendantWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    View.ModalWidgetSlot.Add( widget );
                } else {
                    View.WidgetSlot.Add( widget );
                }
                {
                    var prev = (UIWidgetBase?) View.WidgetSlot.Widgets.Concat( View.ModalWidgetSlot.Widgets ).SkipLast( 1 ).LastOrDefault();
                    if (prev != null) prev.View!.VisualElement.SaveFocus();
                    RecalcVisibility( View );
                    var last = (UIWidgetBase?) View.WidgetSlot.Widgets.Concat( View.ModalWidgetSlot.Widgets ).LastOrDefault();
                    if (last != null) last.View!.VisualElement.Focus2();
                }
            }
        }
        protected override void HideDescendantWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == View.ModalWidgetSlot.Widgets.LastOrDefault() );
                    View.ModalWidgetSlot.Remove( widget );
                } else {
                    Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == View.WidgetSlot.Widgets.LastOrDefault() );
                    View.WidgetSlot.Remove( widget );
                }
                {
                    RecalcVisibility( View );
                    var last = (UIWidgetBase?) View.WidgetSlot.Widgets.Concat( View.ModalWidgetSlot.Widgets ).LastOrDefault();
                    if (last != null) last.View!.VisualElement.LoadFocus();
                }
            }
        }

        // Helpers
        protected static RootWidgetView CreateView() {
            var view = new RootWidgetView();
            view.Widget.OnEvent<NavigationSubmitEvent>( evt => {
                var button = (Button?) evt.target;
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            view.Widget.OnEvent<NavigationCancelEvent>( evt => {
                var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().FirstOrDefault( IsWidget );
                var button = widget?.Query<Button>().Where( IsCancel ).First();
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            return view;
        }
        // Helpers
        protected static void RecalcVisibility(RootWidgetViewBase view) {
            foreach (var widget in view.WidgetSlot.Widgets) {
                RecalcWidgetVisibility( widget, widget == view.WidgetSlot.Widgets.Last(), view.ModalWidgetSlot.Any() );
            }
            foreach (var widget in view.ModalWidgetSlot.Widgets) {
                RecalcModalWidgetVisibility( widget, widget == view.ModalWidgetSlot.Widgets.Last() );
            }
        }
        protected static void RecalcWidgetVisibility(UIWidgetBase widget, bool isVisible, bool hasModalWidgets) {
            if (!isVisible) {
                // hide covered widgets
                widget.SetEnabled( false );
                widget.SetDisplayed( false );
            } else {
                // show new widget or unhide uncovered widget
                widget.SetEnabled( !hasModalWidgets );
                widget.SetDisplayed( true );
            }
        }
        protected static void RecalcModalWidgetVisibility(UIWidgetBase widget, bool isVisible) {
            if (!isVisible) {
                // hide covered widgets
                widget.SetEnabled( false );
                widget.SetDisplayed( false );
            } else {
                // show new widget or unhide uncovered widget
                widget.SetEnabled( true );
                widget.SetDisplayed( true );
            }
        }
        // Helpers
        protected static bool IsWidget(VisualElement element) {
            if (element.enabledInHierarchy) {
                if (element.name != null) {
                    return element.name.Contains( "widget", StringComparison.CurrentCultureIgnoreCase );
                }
                return element.GetType().Name.Contains( "widget", StringComparison.CurrentCultureIgnoreCase );
            }
            return false;
        }
        protected static bool IsCancel(Button button) {
            if (button.enabledInHierarchy) {
                if (button.name != null) {
                    return button.name is "resume" or "cancel" or "no" or "back" or "exit" or "quit";
                }
                if (button.text != null) {
                    return button.text.Contains( "resume", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "cancel", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "no", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "back", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "exit", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "quit", StringComparison.CurrentCultureIgnoreCase );
                }
            }
            return false;
        }
        protected static void Click(Button button) {
            using (var click = ClickEvent.GetPooled()) {
                click.target = button;
                button.SendEvent( click );
            }
        }

    }
}

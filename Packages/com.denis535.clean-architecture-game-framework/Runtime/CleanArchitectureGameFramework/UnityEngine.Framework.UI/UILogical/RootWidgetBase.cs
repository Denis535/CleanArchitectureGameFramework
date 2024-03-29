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
                var covered = (UIWidgetBase?) View.WidgetSlot.Widgets.Concat( View.ModalWidgetSlot.Widgets ).LastOrDefault();
                if (covered != null) covered.__GetView__()!.__GetVisualElement__().SaveFocus();

                if (widget.IsModal()) {
                    ShowDescendantWidget( View.ModalWidgetSlot, widget );
                    View.WidgetSlot.SetEnabled( false );
                } else {
                    ShowDescendantWidget( View.WidgetSlot, widget );
                }

                var @new = (UIWidgetBase?) View.WidgetSlot.Widgets.Concat( View.ModalWidgetSlot.Widgets ).LastOrDefault();
                if (@new != null) @new.__GetView__()!.__GetVisualElement__().Focus2();
            }
        }
        protected override void HideDescendantWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    HideDescendantWidget( View.ModalWidgetSlot, widget );
                    View.WidgetSlot.SetEnabled( !View.ModalWidgetSlot.Widgets.Any() );
                } else {
                    HideDescendantWidget( View.WidgetSlot, widget );
                }

                var uncovered = (UIWidgetBase?) View.WidgetSlot.Widgets.Concat( View.ModalWidgetSlot.Widgets ).LastOrDefault();
                if (uncovered != null) uncovered.__GetView__()!.__GetVisualElement__().LoadFocus();
            }
        }

        // ShowDescendantWidget
        protected virtual void ShowDescendantWidget(WidgetListSlotWrapper<UIWidgetBase> slot, UIWidgetBase widget) {
            var covered = slot.Widgets.LastOrDefault();
            if (covered != null) slot.__GetVisualElement__().Remove( covered.__GetView__()!.__GetVisualElement__() );
            slot.Add( widget );
        }
        protected virtual void HideDescendantWidget(WidgetListSlotWrapper<UIWidgetBase> slot, UIWidgetBase widget) {
            Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == slot.Widgets.LastOrDefault() );
            slot.Remove( widget );
            var uncovered = slot.Widgets.LastOrDefault();
            if (uncovered != null) slot.__GetVisualElement__().Add( uncovered.__GetView__()!.__GetVisualElement__() );
        }

        // Helpers
        protected static RootWidgetView CreateView() {
            var view = new RootWidgetView();
            view.Widget.OnEvent<NavigationSubmitEvent>( evt => {
                var button = evt.target as Button;
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
                    return button.name.Contains( "resume", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.name.Contains( "cancel", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.name.Contains( "no", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.name.Contains( "back", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.name.Contains( "exit", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.name.Contains( "quit", StringComparison.CurrentCultureIgnoreCase );
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
            using (var evt = ClickEvent.GetPooled()) {
                evt.target = button;
                button.SendEvent( evt );
            }
        }

    }
}

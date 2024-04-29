#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetBase<TView> : UIWidgetBase<TView> where TView : UIRootWidgetViewBase {

        // Constructor
        public UIRootWidgetBase() {
        }
        public UIRootWidgetBase(TView view) {
            View = view;
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
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantAttach( descendant, argument );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant, object? argument) {
            base.OnAfterDescendantAttach( descendant, argument );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantDetach( descendant, argument );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant, object? argument) {
            base.OnAfterDescendantDetach( descendant, argument );
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
    public class UIRootWidget : UIRootWidgetBase<UIRootWidgetViewBase> {

        // View
        public IReadOnlyList<UIWidgetBase> Widgets => View.WidgetSlot.Children;
        public IReadOnlyList<UIWidgetBase> ModalWidgets => View.ModalWidgetSlot.Children;

        // Constructor
        public UIRootWidget() {
            View = CreateView();
        }
        public UIRootWidget(UIRootWidgetViewBase view) : base( view ) {
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
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantAttach( descendant, argument );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant, object? argument) {
            base.OnAfterDescendantAttach( descendant, argument );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantDetach( descendant, argument );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant, object? argument) {
            base.OnAfterDescendantDetach( descendant, argument );
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
                var covered = (UIWidgetBase?) View.WidgetSlot.Children.Concat( View.ModalWidgetSlot.Children ).LastOrDefault();
                if (covered != null) covered.__GetView__()!.__GetVisualElement__().SaveFocus();
                if (widget.IsModal()) {
                    View.WidgetSlot.SetEnabled( false );
                    ShowDescendantWidget( View.ModalWidgetSlot, widget );
                } else {
                    ShowDescendantWidget( View.WidgetSlot, widget );
                }
                var @new = (UIWidgetBase?) View.WidgetSlot.Children.Concat( View.ModalWidgetSlot.Children ).LastOrDefault();
                if (@new != null) @new.__GetView__()!.__GetVisualElement__().Focus2();
            }
        }
        protected override void HideDescendantWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    HideDescendantWidget( View.ModalWidgetSlot, widget );
                    View.WidgetSlot.SetEnabled( !View.ModalWidgetSlot.Children.Any() );
                } else {
                    HideDescendantWidget( View.WidgetSlot, widget );
                }
                var uncovered = (UIWidgetBase?) View.WidgetSlot.Children.Concat( View.ModalWidgetSlot.Children ).LastOrDefault();
                if (uncovered != null) uncovered.__GetView__()!.__GetVisualElement__().LoadFocus();
            }
        }

        // ShowDescendantWidget
        protected virtual void ShowDescendantWidget(WidgetListSlotWrapper<UIWidgetBase> slot, UIWidgetBase widget) {
            var covered = slot.Children.LastOrDefault();
            if (covered != null) slot.__GetVisualElement__().Remove( covered.__GetView__()!.__GetVisualElement__() );
            slot.Add( widget );
        }
        protected virtual void HideDescendantWidget(WidgetListSlotWrapper<UIWidgetBase> slot, UIWidgetBase widget) {
            Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == slot.Children.LastOrDefault() );
            slot.Remove( widget );
            var uncovered = slot.Children.LastOrDefault();
            if (uncovered != null) slot.__GetVisualElement__().Add( uncovered.__GetView__()!.__GetVisualElement__() );
        }

        // Helpers
        protected static UIRootWidgetView CreateView() {
            var view = new UIRootWidgetView();
            view.Root.OnEvent<NavigationSubmitEvent>( evt => {
                var button = evt.target as Button;
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            view.Root.OnEvent<NavigationCancelEvent>( evt => {
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

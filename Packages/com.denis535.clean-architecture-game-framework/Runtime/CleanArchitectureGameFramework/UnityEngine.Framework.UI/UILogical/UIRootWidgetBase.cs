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
        public UIRootWidgetBase(TView view) : base( view ) {
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
        public override void AttachChild(UIWidgetBase child, object? argument = null) {
            base.AttachChild( child, argument );
        }
        public override void DetachChild(UIWidgetBase child, object? argument = null) {
            base.DetachChild( child, argument );
        }

        // ShowWidget
        public override void ShowWidget(UIWidgetBase widget) {
            throw Exceptions.Internal.NotImplemented( $"Method 'ShowWidget' is not implemented" );
        }
        public override void HideWidget(UIWidgetBase widget) {
            throw Exceptions.Internal.NotImplemented( $"Method 'HideWidget' is not implemented" );
        }

    }
    public class UIRootWidget : UIRootWidgetBase<UIRootWidgetViewBase> {

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
        public override void AttachChild(UIWidgetBase child, object? argument = null) {
            base.AttachChild( child, argument );
        }
        public override void DetachChild(UIWidgetBase child, object? argument = null) {
            base.DetachChild( child, argument );
        }

        // ShowWidget
        public override void ShowWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    View.WidgetSlot.SetEnabled( false );
                    Push( View.ModalWidgetSlot, widget, i => true );
                } else {
                    Push( View.WidgetSlot, widget, i => true );
                }
            }
        }
        public override void HideWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    Pop( View.ModalWidgetSlot, widget, i => true );
                    View.WidgetSlot.SetEnabled( !View.ModalWidgetSlot.Children.Any() );
                } else {
                    Pop( View.WidgetSlot, widget, i => true );
                }
            }
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
        // Helpers
        protected static void Push(WidgetListSlotWrapper<UIWidgetBase> slot, UIWidgetBase widget, Func<UIWidgetBase, bool> canBeHidden) {
            var last = slot.Children.LastOrDefault();
            if (last != null && canBeHidden( last )) slot.__GetVisualElement__().Remove( last.View!.__GetVisualElement__() );
            slot.Add( widget );
        }
        protected static void Pop(WidgetListSlotWrapper<UIWidgetBase> slot, UIWidgetBase widget, Func<UIWidgetBase, bool> canBeHidden) {
            Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == slot.Children.LastOrDefault() );
            slot.Remove( widget );
            var last = slot.Children.LastOrDefault();
            if (last != null && canBeHidden( last )) slot.__GetVisualElement__().Add( last.View!.__GetVisualElement__() );
        }

    }
}

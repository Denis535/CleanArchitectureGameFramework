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
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach(object? argument) {
        }
        public override void OnDetach(object? argument) {
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantAttach( descendant, argument );
            // override here
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant, object? argument) {
            // override here
            base.OnAfterDescendantAttach( descendant, argument );
        }

        // OnDescendantDetach
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantDetach( descendant, argument );
            // override here
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant, object? argument) {
            // override here
            base.OnAfterDescendantDetach( descendant, argument );
        }

        // AttachChild
        public override void AttachChild(UIWidgetBase child, object? argument = null) {
            base.AttachChild( child, argument );
        }
        public override void DetachChild(UIWidgetBase child, object? argument = null) {
            base.DetachChild( child, argument );
        }

        // ShowView
        public override void ShowView(UIViewBase view) {
            throw Exceptions.Internal.NotImplemented( $"Method 'ShowView' is not implemented" );
        }
        public override void HideView(UIViewBase view) {
            throw Exceptions.Internal.NotImplemented( $"Method 'HideView' is not implemented" );
        }

    }
    public class UIRootWidget : UIRootWidgetBase<UIRootWidgetViewBase> {

        // View
        public override UIRootWidgetViewBase View { get; }

        // Constructor
        public UIRootWidget() {
            View = CreateView();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach(object? argument) {
        }
        public override void OnDetach(object? argument) {
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantAttach( descendant, argument );
            // override here
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant, object? argument) {
            // override here
            base.OnAfterDescendantAttach( descendant, argument );
        }

        // OnDescendantDetach
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant, object? argument) {
            base.OnBeforeDescendantDetach( descendant, argument );
            // override here
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant, object? argument) {
            // override here
            base.OnAfterDescendantDetach( descendant, argument );
        }

        // AttachChild
        public override void AttachChild(UIWidgetBase child, object? argument = null) {
            base.AttachChild( child, argument );
        }
        public override void DetachChild(UIWidgetBase child, object? argument = null) {
            base.DetachChild( child, argument );
        }

        // ShowView
        public override void ShowView(UIViewBase view) {
            if (!view.IsModal()) {
                View.AddView( view );
            } else {
                View.AddModalView( view );
            }
        }
        public override void HideView(UIViewBase view) {
            if (!view.IsModal()) {
                View.RemoveView( view );
            } else {
                View.RemoveModalView( view );
            }
        }

        // Helpers
        protected static UIRootWidgetView CreateView() {
            var view = new UIRootWidgetView();
            view.OnSubmit( evt => {
                var button = evt.target as Button;
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            } );
            view.OnCancel( evt => {
                var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().FirstOrDefault( IsWidget );
                var button = widget?.Query<Button>().Where( IsCancel ).First();
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            } );
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

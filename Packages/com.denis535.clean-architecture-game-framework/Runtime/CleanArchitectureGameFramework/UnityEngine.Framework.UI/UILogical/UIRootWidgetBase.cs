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

        // CreateView
        protected virtual UIRootWidgetViewBase CreateView() {
            var view = new UIRootWidgetView();
            view.OnSubmit( evt => {
                var button = evt.target as Button;
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            } );
            view.OnCancel( evt => {
                var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().Where( i => i.IsAttached() && i.enabledInHierarchy && i.IsDisplayedInHierarchy() ).FirstOrDefault( IsWidget );
                var button = widget?.Query<Button>().Where( i => i.IsAttached() && i.enabledInHierarchy && i.IsDisplayedInHierarchy() ).Where( IsCancel ).First();
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            } );
            return view;
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
                View.AddView( view, i => false );
            } else {
                View.AddModalView( view, i => false );
            }
        }
        public override void HideView(UIViewBase view) {
            if (!view.IsModal()) {
                View.RemoveView( view, i => false );
            } else {
                View.RemoveModalView( view, i => false );
            }
        }

        // Helpers
        protected static bool IsWidget(VisualElement element) {
            return element.GetClasses().Any( i => i.Contains( "widget" ) );
        }
        protected static bool IsCancel(Button button) {
            return button.ClassListContains( "resume" ) ||
                button.ClassListContains( "cancel" ) ||
                button.ClassListContains( "back" ) ||
                button.ClassListContains( "no" ) ||
                button.ClassListContains( "exit" ) ||
                button.ClassListContains( "quit" );
        }
        protected static void Click(Button button) {
            using (var evt = ClickEvent.GetPooled()) {
                evt.target = button;
                button.SendEvent( evt );
            }
        }

    }
}

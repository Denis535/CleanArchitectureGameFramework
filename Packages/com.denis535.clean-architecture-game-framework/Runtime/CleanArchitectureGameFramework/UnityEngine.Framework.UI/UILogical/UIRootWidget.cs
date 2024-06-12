#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIRootWidget : UIWidgetBase<UIRootWidgetView> {

        // View
        public override UIRootWidgetView View { get; }

        // Constructor
        public UIRootWidget() {
            View = CreateView<UIRootWidgetView>();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnActivate
        protected override void OnActivate(object? argument) {
        }
        protected override void OnDeactivate(object? argument) {
        }

        // OnDescendantActivate
        protected override void OnBeforeDescendantActivate(UIWidgetBase descendant, object? argument) {
        }
        protected override void OnAfterDescendantActivate(UIWidgetBase descendant, object? argument) {
        }
        protected override void OnBeforeDescendantDeactivate(UIWidgetBase descendant, object? argument) {
        }
        protected override void OnAfterDescendantDeactivate(UIWidgetBase descendant, object? argument) {
        }

        // ShowView
        protected override void ShowView(UIViewBase view) {
            View.AddView( view );
        }
        protected override void HideView(UIViewBase view) {
            View.RemoveView( view );
        }

        // Helpers
        protected static T CreateView<T>() where T : UIRootWidgetView, new() {
            var view = new T();
            view.OnSubmit += OnSubmit;
            view.OnCancel += OnCancel;
            return view;
        }
        protected static void OnSubmit(NavigationSubmitEvent evt) {
            var button = evt.target as Button;
            if (button != null) {
                Click( button );
                evt.StopPropagation();
            }
        }
        protected static void OnCancel(NavigationCancelEvent evt) {
            var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().Where( i => i.IsAttached() && i.enabledInHierarchy && i.IsDisplayedInHierarchy() ).FirstOrDefault( IsWidget );
            var button = widget?.Query<Button>().Where( i => i.IsAttached() && i.enabledInHierarchy && i.IsDisplayedInHierarchy() ).Where( IsCancel ).First();
            if (button != null) {
                Click( button );
                evt.StopPropagation();
            }
        }
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

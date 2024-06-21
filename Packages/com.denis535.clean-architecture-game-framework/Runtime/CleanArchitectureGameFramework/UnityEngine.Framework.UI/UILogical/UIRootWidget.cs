#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidget<TView> : UIWidgetBase2<TView> where TView : UIRootWidgetView {

        // Constructor
        public UIRootWidget(IDependencyContainer container) : base( container ) {
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
            View.AddView( (UIViewBase2) view );
        }
        protected override void HideView(UIViewBase view) {
            View.RemoveView( (UIViewBase2) view );
        }

        // Helpers
        protected static T CreateView<T>() where T : UIRootWidgetView, new() {
            var view = new T();
            view.OnSubmitEvent += OnSubmit;
            view.OnCancelEvent += OnCancel;
            return view;
        }
        // Helpers
        protected static void OnSubmit(NavigationSubmitEvent evt) {
            var button = evt.target as Button;
            if (button != null) {
                Click( button );
                evt.StopPropagation();
            }
        }
        protected static void OnCancel(NavigationCancelEvent evt) {
            var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().Where( i => i.enabledInHierarchy && i.IsDisplayedInHierarchy() ).FirstOrDefault( IsWidget );
            var button = widget?.Query<Button>().Where( i => i.enabledInHierarchy && i.IsDisplayedInHierarchy() ).Where( IsCancel ).First();
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
    public class UIRootWidget : UIRootWidget<UIRootWidgetView> {

        // View
        public override UIRootWidgetView View { get; }

        // Constructor
        public UIRootWidget(IDependencyContainer container) : base( container ) {
            View = CreateView<UIRootWidgetView>();
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

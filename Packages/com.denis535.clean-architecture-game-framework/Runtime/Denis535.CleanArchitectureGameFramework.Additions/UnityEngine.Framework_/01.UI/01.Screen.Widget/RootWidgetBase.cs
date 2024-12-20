#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class RootWidgetBase<TView> : WidgetBase2<TView> where TView : RootWidgetViewBase {

        // Constructor
        public RootWidgetBase(IDependencyContainer container) : base( container ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnActivate
        protected override void OnActivate(object? argument) {
            ShowSelf();
        }
        protected override void OnDeactivate(object? argument) {
            HideSelf();
        }

        // Sort
        protected override void Sort(List<WidgetBase> children) {
            children.Sort( (a, b) => Comparer<int>.Default.Compare( GetOrderOf( a ), GetOrderOf( b ) ) );
        }
        protected virtual int GetOrderOf(WidgetBase widget) {
            return 0;
        }

        // ShowSelf
        protected override void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be activating" ).Valid( Activity is Activity_.Activating );
            Screen!.Document.rootVisualElement.Add( View );
        }
        protected override void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be deactivating" ).Valid( Activity is Activity_.Deactivating );
            if (Screen!.Document && Screen!.Document.rootVisualElement != null) Screen!.Document.rootVisualElement.Remove( View );
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
            var widget = ((VisualElement) evt.target).AncestorsAndSelf().LastOrDefault( IsWidget );
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
                button.ClassListContains( "quit" ) ||
                button.ClassListContains( "close" );
        }
        protected static void Click(Button button) {
            using (var evt = ClickEvent.GetPooled()) {
                evt.target = button;
                button.SendEvent( evt );
            }
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        protected readonly VisualElement widget;

        // Children
        public IEnumerable<UIViewBase> Children => widget.Children().Select( i => i.GetView() );

        // Constructor
        public UIRootWidgetViewBase() {
            VisualElement = CreateVisualElement( out widget );
        }
        public override void Dispose() {
            base.Dispose();
        }

        // CreateVisualElement
        protected virtual VisualElement CreateVisualElement(out VisualElement widget) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "widget" );
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            return widget;
        }

        // AddView
        public virtual void AddView(UIViewBase view) {
            widget.Add( view );
            widget.Sort( (a, b) => Comparer<int>.Default.Compare( a.GetView().Priority, b.GetView().Priority ) );
        }
        public virtual void RemoveView(UIViewBase view) {
            widget.Remove( view );
        }

        // OnEvent
        public void OnSubmit(EventCallback<NavigationSubmitEvent> callback) {
            widget.OnSubmit( callback, TrickleDown.TrickleDown );
        }
        public void OnCancel(EventCallback<NavigationCancelEvent> callback) {
            widget.OnCancel( callback, TrickleDown.TrickleDown );
        }

    }
    public class UIRootWidgetView : UIRootWidgetViewBase {

        // Priority
        public override int Priority => 0;
        // IsAlwaysVisible
        public override bool IsAlwaysVisible => false;
        // IsModal
        public override bool IsModal => false;

        // Constructor
        public UIRootWidgetView() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddView
        public override void AddView(UIViewBase view) {
            base.AddView( view );
            Recalculate( Children.ToArray() );
        }
        public override void RemoveView(UIViewBase view) {
            base.RemoveView( view );
            Recalculate( Children.ToArray() );
        }

        // Recalculate
        protected virtual void Recalculate(UIViewBase[] views) {
            for (var i = 0; i < views.Length; i++) {
                var view = views[ i ];
                var next = views.ElementAtOrDefault( i + 1 );
                if (next == null) {
                    ShowView( view );
                } else {
                    HideView( view, next );
                }
            }
        }
        protected virtual void ShowView(UIViewBase view) {
            if (!view.IsAlwaysVisible) {
                view.VisualElement.SetEnabled( true );
                view.VisualElement.SetDisplayed( true );
            }
            if (!HasFocusedElement( view.VisualElement )) {
                if (!LoadFocus( view )) {
                    Focus( view );
                }
            }
        }
        protected virtual void HideView(UIViewBase view, UIViewBase next) {
            if (HasFocusedElement( view.VisualElement )) {
                SaveFocus( view );
            }
            if (!view.IsAlwaysVisible) {
                if (!view.IsModal && next.IsModal) {
                    view.VisualElement.SetEnabled( false );
                } else {
                    view.VisualElement.SetDisplayed( false );
                }
            }
        }

        // Focus
        protected virtual void Focus(UIViewBase view) {
            try {
                if (view.VisualElement.focusable) {
                    view.VisualElement.Focus();
                } else {
                    view.VisualElement.focusable = true;
                    view.VisualElement.delegatesFocus = true;
                    view.VisualElement.Focus();
                    view.VisualElement.delegatesFocus = false;
                    view.VisualElement.focusable = false;
                }
            } catch (Exception ex) {
                Debug.LogWarning( ex );
            }
        }
        protected virtual bool LoadFocus(UIViewBase view) {
            var focusedElement = view.LoadFocusedElement();
            if (focusedElement != null) {
                focusedElement.Focus();
                return true;
            }
            return false;
        }
        protected virtual void SaveFocus(UIViewBase view) {
            var focusedElement = GetFocusedElement( view );
            view.SaveFocusedElement( focusedElement );
        }

        // Helpers
        protected static VisualElement? GetFocusedElement(UIViewBase view) {
            var focusedElement = (VisualElement) view.VisualElement.focusController.focusedElement;
            if (focusedElement != null && (view.VisualElement == focusedElement || view.VisualElement.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        protected static bool HasFocusedElement(VisualElement visualElement) {
            var focusedElement = (VisualElement) visualElement.focusController.focusedElement;
            if (focusedElement != null && (visualElement == focusedElement || visualElement.Contains( focusedElement ))) return true;
            return false;
        }

    }
}

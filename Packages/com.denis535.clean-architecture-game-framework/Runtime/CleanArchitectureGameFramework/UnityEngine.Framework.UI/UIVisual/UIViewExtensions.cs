#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static partial class UIViewExtensions {

        // GetVisualElement
        public static VisualElement __GetVisualElement__(this UIViewBase2 view) {
            // try not to use this method
            return view.VisualElement;
        }
        public static T __GetVisualElement__<T>(this UIViewBase2 view) where T : VisualElement {
            // try not to use this method
            return (T) view.VisualElement;
        }

        // Focus
        public static void Focus(this UIViewBase2 view) {
            try {
                if (view.VisualElement.focusable) {
                    view.VisualElement.Focus();
                } else {
                    view.VisualElement.focusable = true;
                    view.VisualElement.delegatesFocus = true;
                    view.VisualElement.Focus(); // sometimes it throws an error
                    view.VisualElement.delegatesFocus = false;
                    view.VisualElement.focusable = false;
                }
            } catch {
            }
        }
        public static bool LoadFocus(this UIViewBase2 view) {
            var focusedElement = view.LoadFocusedElement();
            if (focusedElement != null) {
                focusedElement.Focus();
                return true;
            }
            return false;
        }
        public static void SaveFocus(this UIViewBase2 view) {
            var focusedElement = GetFocusedElement( view );
            view.SaveFocusedElement( focusedElement );
        }

        // GetFocusedElement
        public static VisualElement? GetFocusedElement(this UIViewBase2 view) {
            var focusedElement = (VisualElement?) view.VisualElement.focusController?.focusedElement;
            if (focusedElement != null && (view.VisualElement == focusedElement || view.VisualElement.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        public static bool HasFocusedElement(this UIViewBase2 view) {
            var focusedElement = (VisualElement?) view.VisualElement.focusController?.focusedElement;
            if (focusedElement != null && (view.VisualElement == focusedElement || view.VisualElement.Contains( focusedElement ))) return true;
            return false;
        }

    }
    public static partial class UIViewExtensions {

        // GetView
        public static UIViewBase2 GetView(this VisualElement element) {
            return (UIViewBase2) element.userData ?? throw Exceptions.Operation.InvalidOperationException( $"Element {element} must have view" );
        }
        public static T GetView<T>(this VisualElement element) where T : UIViewBase2 {
            return (T) element.userData ?? throw Exceptions.Operation.InvalidOperationException( $"Element {element} must have {typeof( T )} view" );
        }

        // Children
        public static IEnumerable<UIViewBase2> Children2(this VisualElement element) {
            return element.Children().Select( i => i.GetView() );
        }
        public static IEnumerable<T> Children2<T>(this VisualElement element) where T : UIViewBase2 {
            return element.Children().Select( i => i.GetView<T>() );
        }

        // Add
        public static void Add(this VisualElement element, UIViewBase2 child) {
            element.Add( child.VisualElement );
        }
        public static void Remove(this VisualElement element, UIViewBase2 child) {
            element.Remove( child.VisualElement );
        }
        public static bool Contains(this VisualElement element, UIViewBase2 child) {
            return element.Contains( child.VisualElement );
        }

    }
}

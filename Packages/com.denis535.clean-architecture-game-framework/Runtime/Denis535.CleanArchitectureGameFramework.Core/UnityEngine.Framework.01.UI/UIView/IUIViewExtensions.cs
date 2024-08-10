#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class IUIViewExtensions {

        // GetParent
        public static IUIView? GetParent(this IUIView view) {
            return GetParent( (VisualElement) view );
            static IUIView? GetParent(VisualElement element) {
                if (element.parent is IUIView parent_) {
                    return parent_;
                }
                if (element.parent != null) {
                    return GetParent( element.parent );
                }
                return null;
            }
        }

        // GetChildren
        public static IEnumerable<IUIView> GetChildren(this IUIView view) {
            return GetChildren( (VisualElement) view );
            static IEnumerable<IUIView> GetChildren(VisualElement element) {
                foreach (var child in element.Children()) {
                    if (child is IUIView child_) {
                        yield return child_;
                    } else {
                        foreach (var i in GetChildren( child )) yield return i;
                    }
                }
            }
        }

        // Focus
        public static void InitFocus(this VisualElement view) {
            try {
                if (view.focusable) {
                    view.Focus();
                } else {
                    view.focusable = true;
                    view.delegatesFocus = true;
                    view.Focus(); // sometimes it throws an error
                    view.delegatesFocus = false;
                    view.focusable = false;
                }
            } catch {
            }
        }
        public static bool LoadFocus(this VisualElement view) {
            var focusedElement = view.LoadFocusedElement();
            if (focusedElement != null) {
                focusedElement.Focus();
                return true;
            }
            return false;
        }
        public static void SaveFocus(this VisualElement view) {
            var focusedElement = GetFocusedElement( view );
            view.SaveFocusedElement( focusedElement );
        }

        // GetFocusedElement
        public static VisualElement? GetFocusedElement(this VisualElement view) {
            var focusedElement = (VisualElement?) view.focusController?.focusedElement;
            if (focusedElement != null && (view == focusedElement || view.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        public static bool HasFocusedElement(this VisualElement view) {
            var focusedElement = (VisualElement?) view.focusController?.focusedElement;
            if (focusedElement != null && (view == focusedElement || view.Contains( focusedElement ))) return true;
            return false;
        }

        // LoadFocusedElement
        public static VisualElement? LoadFocusedElement(this VisualElement view) {
            return (VisualElement) view.userData;
        }
        public static void SaveFocusedElement(this VisualElement view, VisualElement? focusedElement) {
            view.userData = focusedElement;
        }

    }
}

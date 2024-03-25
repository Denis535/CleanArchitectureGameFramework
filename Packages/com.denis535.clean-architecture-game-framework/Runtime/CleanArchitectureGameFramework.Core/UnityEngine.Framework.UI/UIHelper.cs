#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    internal static class UIHelper {

        // GetAncestors
        public static IReadOnlyList<UIWidgetBase> GetAncestors(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            while (widget.Parent != null) {
                result.Add( widget.Parent );
                widget = widget.Parent;
            }
            return result;
        }
        public static IReadOnlyList<UIWidgetBase> GetAncestorsAndSelf(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            result.Add( widget );
            GetAncestors( widget, result );
            return result;
        }

        // GetDescendants
        public static IReadOnlyList<UIWidgetBase> GetDescendants(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            foreach (var child in widget.Children) {
                result.Add( child );
                GetDescendants( child, result );
            }
            return result;
        }
        public static IReadOnlyList<UIWidgetBase> GetDescendantsAndSelf(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            result.Add( widget );
            GetDescendants( widget, result );
            return result;
        }

        // Add
        public static void Add(this VisualElement element, UIWidgetBase widget) {
            element.Add( widget.View!.VisualElement );
        }
        public static void Remove(this VisualElement element, UIWidgetBase widget) {
            element.Remove( widget.View!.VisualElement );
        }
        public static void Contains(this VisualElement element, UIWidgetBase widget) {
            element.Contains( widget.View!.VisualElement );
        }

        // Add
        public static void Add(this VisualElement element, UIViewBase view) {
            element.Add( view!.VisualElement );
        }
        public static void Remove(this VisualElement element, UIViewBase view) {
            element.Remove( view!.VisualElement );
        }
        public static void Contains(this VisualElement element, UIViewBase view) {
            element.Contains( view!.VisualElement );
        }

    }
}

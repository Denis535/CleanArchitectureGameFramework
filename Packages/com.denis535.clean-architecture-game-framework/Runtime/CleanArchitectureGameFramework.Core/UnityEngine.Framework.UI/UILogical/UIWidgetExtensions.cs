#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class UIWidgetExtensions {

        // GetAncestors
        public static IEnumerable<UIWidgetBase> GetAncestors(this UIWidgetBase widget) {
            while (widget.Parent != null) {
                yield return widget.Parent;
                widget = widget.Parent;
            }
        }
        public static IEnumerable<UIWidgetBase> GetAncestorsAndSelf(this UIWidgetBase widget) {
            yield return widget;
            foreach (var i in GetAncestors( widget )) yield return i;
        }

        // GetDescendants
        public static IEnumerable<UIWidgetBase> GetDescendants(this UIWidgetBase widget) {
            foreach (var child in widget.Children) {
                yield return child;
                foreach (var i in GetDescendants( child )) yield return i;
            }
        }
        public static IEnumerable<UIWidgetBase> GetDescendantsAndSelf(this UIWidgetBase widget) {
            yield return widget;
            foreach (var i in GetDescendants( widget )) yield return i;
        }

        // OnActivate
        public static void OnBeforeActivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBeforeActivateEvent += callback;
        }
        public static void OnAfterActivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfterActivateEvent += callback;
        }
        public static void OnBeforeDeactivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBeforeDeactivateEvent += callback;
        }
        public static void OnAfterDeactivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfterDeactivateEvent += callback;
        }

        // OnDescendantActivate
        public static void OnBeforeDescendantActivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBeforeDescendantActivateEvent += callback;
        }
        public static void OnAfterDescendantActivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfterDescendantActivateEvent += callback;
        }
        public static void OnBeforeDescendantDeactivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBeforeDescendantDeactivateEvent += callback;
        }
        public static void OnAfterDescendantDeactivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfterDescendantDeactivateEvent += callback;
        }

    }
}

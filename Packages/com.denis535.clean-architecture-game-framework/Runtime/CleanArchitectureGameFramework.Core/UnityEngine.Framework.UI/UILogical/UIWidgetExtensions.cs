#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class UIWidgetExtensions {

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

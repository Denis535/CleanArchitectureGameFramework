#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class UIHelper {

        // GetAncestors
        public static List<UIWidgetBase> GetAncestors(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            while (widget.Parent != null) {
                result.Add( widget.Parent );
                widget = widget.Parent;
            }
            return result;
        }
        public static List<UIWidgetBase> GetAncestorsAndSelf(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            result.Add( widget );
            GetAncestors( widget, result );
            return result;
        }

        // GetDescendants
        public static List<UIWidgetBase> GetDescendants(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            foreach (var child in widget.Children) {
                result.Add( child );
                GetDescendants( child, result );
            }
            return result;
        }
        public static List<UIWidgetBase> GetDescendantsAndSelf(this UIWidgetBase widget, List<UIWidgetBase>? result = null) {
            result ??= new List<UIWidgetBase>();
            result.Add( widget );
            GetDescendants( widget, result );
            return result;
        }

    }
}

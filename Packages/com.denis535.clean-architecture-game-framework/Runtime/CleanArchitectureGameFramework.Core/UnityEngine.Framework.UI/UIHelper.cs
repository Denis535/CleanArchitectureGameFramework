#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    internal static class UIHelper {

        // Add
        public static void Add(this VisualElement element, UIViewBase view) {
            element.Add( view.VisualElement );
        }
        public static void Remove(this VisualElement element, UIViewBase view) {
            element.Remove( view.VisualElement );
        }
        public static void Contains(this VisualElement element, UIViewBase view) {
            element.Contains( view.VisualElement );
        }

    }
}

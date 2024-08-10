#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class UIWidgetExtensions {

        // GetView
        public static IUIView? __GetView__(this UIWidgetBase widget) {
            // try not to use this method
            return widget.View;
        }
        public static T __GetView__<T>(this UIWidgetBase<T> widget) where T : notnull, VisualElement, IUIView {
            // try not to use this method
            return widget.View;
        }

    }
}

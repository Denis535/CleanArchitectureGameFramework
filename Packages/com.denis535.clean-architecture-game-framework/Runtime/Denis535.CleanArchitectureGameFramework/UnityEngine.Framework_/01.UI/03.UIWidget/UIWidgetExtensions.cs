#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class UIWidgetExtensions {

        // GetView
        public static UIViewBase? __GetView__(this UIWidgetBase widget) {
            // try not to use this method
            return widget.View;
        }
        public static T __GetView__<T>(this UIWidgetBase<T> widget) where T : notnull, UIViewBase {
            // try not to use this method
            return widget.View;
        }

    }
}

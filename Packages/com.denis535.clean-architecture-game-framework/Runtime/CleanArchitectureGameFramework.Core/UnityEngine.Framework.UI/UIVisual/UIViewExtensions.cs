#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class UIViewExtensions {

        // GetVisualElement
        public static VisualElement __GetVisualElement__(this UIViewBase view) {
            // try not to use VisualElement outside of it's view
            return view.VisualElement;
        }

    }
}

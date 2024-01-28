#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class UIViewExtensions {

        // GetVisualElement
        public static VisualElement GetVisualElement(this UIViewBase view) {
            return view.VisualElement;
        }

    }
}

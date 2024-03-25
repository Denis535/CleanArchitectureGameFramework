#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class UIViewExtensions {

        // IsEnabled
        public static bool IsEnabledInHierarchy(this UIViewBase view) {
            return view.VisualElement.enabledInHierarchy;
        }
        public static bool IsEnabledSelf(this UIViewBase view) {
            return view.VisualElement.enabledSelf;
        }
        public static void SetEnabled(this UIViewBase view, bool value) {
            view.VisualElement.SetEnabled( value );
        }

        // IsDisplayed
        public static bool IsDisplayed(this UIViewBase view) {
            return view.VisualElement.IsDisplayed();
        }
        public static void SetDisplayed(this UIViewBase view, bool value) {
            view.VisualElement.SetDisplayed( value );
        }

        // IsValid
        public static bool IsValid(this UIViewBase view) {
            return view.VisualElement.IsValid();
        }
        public static void SetValid(this UIViewBase view, bool value) {
            view.VisualElement.SetValid( value );
        }

        // GetVisualElement
        public static VisualElement __GetVisualElement__(this UIViewBase view) {
            // try not to use it
            return view.VisualElement;
        }

    }
}

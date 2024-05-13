#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class EventExtensions {

        // GetTarget
        public static VisualElement GetTarget(this EventBase @event) {
            return (VisualElement) @event.target;
        }
        public static VisualElement GetTarget<T>(this EventBase @event) where T : VisualElement {
            return (T) @event.target;
        }

    }
}

#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class VisualElementScope : IDisposable {

        private static Stack<VisualElementScope> Stack { get; } = new Stack<VisualElementScope>();
        public static VisualElementScope? Peek => Stack.Any() ? Stack.Peek() : null;

        // VisualElement
        public VisualElement VisualElement { get; }

        // Constructor
        public VisualElementScope(VisualElement visualElement) {
            VisualElement = visualElement;
            Stack.Push( this );
        }
        public void Dispose() {
            Stack.Pop();
        }

    }
    public class VisualElementScope<T> : VisualElementScope where T : VisualElement {

        // VisualElement
        public new T VisualElement => (T) base.VisualElement;

        // Constructor
        public VisualElementScope(T visualElement) : base( visualElement ) {
        }

    }
    public static class VisualElementScopeExtensions {

        // AsScope
        public static VisualElementScope<T> AsScope<T>(this T visualElement) where T : VisualElement {
            VisualElementScope.Peek?.VisualElement.Add( visualElement );
            return new VisualElementScope<T>( visualElement );
        }
        public static VisualElementScope<T> AsScope<T>(this T visualElement, out T @out) where T : VisualElement {
            @out = visualElement;
            VisualElementScope.Peek?.VisualElement.Add( visualElement );
            return new VisualElementScope<T>( visualElement );
        }

        // AddToScope
        public static void AddToScope<T>(this T visualElement) where T : VisualElement {
            VisualElementScope.Peek!.VisualElement.Add( visualElement );
        }
        public static void AddToScope<T>(this T visualElement, out T @out) where T : VisualElement {
            @out = visualElement;
            VisualElementScope.Peek!.VisualElement.Add( visualElement );
        }

    }
}

#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class VisualElementScope : IDisposable {

        private static Stack<VisualElementScope> Stack { get; } = new Stack<VisualElementScope>();
        internal static VisualElementScope? Peek => Stack.Count > 0 ? Stack.Peek() : null;

        public VisualElement VisualElement { get; }

        public VisualElementScope(VisualElement visualElement) {
            VisualElement = visualElement;
            Stack.Push( this );
        }
        public void Dispose() {
            Stack.Pop();
        }

        public void Add(VisualElement element) {
            VisualElement.Add( element );
        }
        public void Remove(VisualElement element) {
            VisualElement.Remove( element );
        }

    }
    public class VisualElementScope<T> : VisualElementScope where T : VisualElement {

        public new T VisualElement => (T) base.VisualElement;

        public VisualElementScope(T visualElement) : base( visualElement ) {
        }

    }
    public static class VisualElementScopeExtensions {

        public static VisualElementScope<T> AsScope<T>(this T visualElement) where T : VisualElement {
            VisualElementScope.Peek?.VisualElement.Add( visualElement );
            return new VisualElementScope<T>( visualElement );
        }
        public static VisualElementScope<T> AsScope<T>(this T visualElement, out T @out) where T : VisualElement {
            @out = visualElement;
            VisualElementScope.Peek?.VisualElement.Add( visualElement );
            return new VisualElementScope<T>( visualElement );
        }

        public static void AddToScope<T>(this T visualElement) where T : VisualElement {
            VisualElementScope.Peek!.VisualElement.Add( visualElement );
        }
        public static void AddToScope<T>(this T visualElement, out T @out) where T : VisualElement {
            @out = visualElement;
            VisualElementScope.Peek!.VisualElement.Add( visualElement );
        }

    }
}

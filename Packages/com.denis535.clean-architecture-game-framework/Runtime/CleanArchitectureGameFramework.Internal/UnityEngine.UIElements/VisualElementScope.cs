#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class VisualElementScope : IDisposable {

        private static readonly Stack<VisualElementScope> stack = new Stack<VisualElementScope>();
        public static VisualElementScope? Current => stack.Count > 0 ? stack.Peek() : null;
        public VisualElement VisualElement { get; }

        public VisualElementScope(VisualElement visualElement) {
            VisualElement = visualElement;
            stack.Push( this );
        }
        public void Dispose() {
            stack.Pop();
        }

        public static VisualElementScope operator +(VisualElementScope? scope, VisualElement element) {
            scope!.VisualElement.Add( element );
            return scope;
        }
        public static VisualElementScope operator -(VisualElementScope? scope, VisualElement element) {
            scope!.VisualElement.Remove( element );
            return scope;
        }

    }
    public class VisualElementScope<T> : VisualElementScope where T : VisualElement {

        public new T VisualElement => (T) base.VisualElement;

        public VisualElementScope(T visualElement) : base( visualElement ) {
        }

    }
    public static class VisualElementScopeExtensions {

        public static VisualElementScope<T> AsScope<T>(this T visualElement) where T : VisualElement {
            VisualElementScope.Current?.VisualElement.Add( visualElement );
            return new VisualElementScope<T>( visualElement );
        }
        public static VisualElementScope<T> AsScope<T>(this T visualElement, out T @out) where T : VisualElement {
            VisualElementScope.Current?.VisualElement.Add( visualElement );
            return new VisualElementScope<T>( @out = visualElement );
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ViewSlotWrapper<T> : VisualElementSlotWrapper where T : notnull, UIViewBase {

        public T? Child { get; private set; }

        public ViewSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Set(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have no child" ).Valid( Child == null );
            Child = child;
            VisualElement.Add( child );
        }
        public void Clear() {
            Assert.Operation.Message( $"Slot must have child" ).Valid( Child != null );
            VisualElement.Remove( Child );
            Child = null;
        }
        public void Clear(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have child" ).Valid( Child != null );
            Assert.Operation.Message( $"Slot must have {child} child" ).Valid( Child == child );
            VisualElement.Remove( Child );
            Child = null;
        }

    }
    public class ViewListSlotWrapper<T> : VisualElementSlotWrapper where T : notnull, UIViewBase {

        private List<T> Children_ { get; } = new List<T>();
        public IReadOnlyList<T> Children => Children_;

        public ViewListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have no {child} child" ).Valid( !Children_.Contains( child ) );
            Children_.Add( child );
            VisualElement.Add( child );
        }
        public void Remove(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have child" ).Valid( Children_.Any() );
            Assert.Operation.Message( $"Slot must have {child} child" ).Valid( Children_.Contains( child ) );
            VisualElement.Remove( child );
            Children_.Remove( child );
        }
        public void Clear() {
            foreach (var child in Children_) {
                VisualElement.Remove( child );
            }
            Children_.Clear();
        }

    }
    public class ViewStackSlotWrapper<T> : VisualElementSlotWrapper where T : notnull, UIViewBase {

        private Stack<T> Children_ { get; } = new Stack<T>();
        public IReadOnlyCollection<T> Children => Children_;

        public ViewStackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Push(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have no {child} child" ).Valid( !Children_.Contains( child ) );
            BeforePush( VisualElement, Children_ );
            {
                Children_.Push( child );
                VisualElement.Add( child );
            }
        }
        public T Peek() {
            Assert.Operation.Message( $"Slot must have child" ).Valid( Children_.Any() );
            var result = Children_.Peek();
            return result;
        }
        public T Pop() {
            Assert.Operation.Message( $"Slot must have child" ).Valid( Children_.Any() );
            var result = Children_.Peek();
            {
                VisualElement.Remove( result );
                Children_.Pop();
            }
            AfterPop( VisualElement, Children_ );
            return result;
        }

        // helpers
        private static void BeforePush(VisualElement element, Stack<T> children) {
            if (children.TryPeek( out var last )) {
                element.Remove( last );
            }
        }
        private static void AfterPop(VisualElement element, Stack<T> children) {
            if (children.TryPeek( out var last )) {
                element.Add( last );
            }
        }

    }
}

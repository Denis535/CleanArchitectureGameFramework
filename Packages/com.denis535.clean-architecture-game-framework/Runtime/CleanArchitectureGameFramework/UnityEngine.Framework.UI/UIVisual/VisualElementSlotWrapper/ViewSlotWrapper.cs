#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ViewSlotWrapper<T> : SlotWrapper<T> where T : notnull, UIViewBase {

        private T? Child_ { get; set; }
        public override T? Child => Child_;

        public ViewSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public override void Set(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have no child" ).Valid( Child == null );
            Child_ = child;
            VisualElement.Add( child );
        }
        public override void Clear() {
            if (Child != null) {
                VisualElement.Remove( Child );
                Child_ = null;
            }
        }

    }
    public class ViewListSlotWrapper<T> : ListSlotWrapper<T> where T : notnull, UIViewBase {

        private List<T> Children_ { get; } = new List<T>();
        public override IReadOnlyList<T> Children => Children_;

        public ViewListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public override void Add(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have no {child} child" ).Valid( !Children_.Contains( child ) );
            Children_.Add( child );
            VisualElement.Add( child );
        }
        public override void Remove(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have {child} child" ).Valid( Children_.Contains( child ) );
            VisualElement.Remove( child );
            Children_.Remove( child );
        }
        public override void Clear() {
            foreach (var child in Children_) {
                VisualElement.Remove( child );
            }
            Children_.Clear();
        }

    }
    public class ViewStackSlotWrapper<T> : StackSlotWrapper<T> where T : notnull, UIViewBase {

        private Stack<T> Children_ { get; } = new Stack<T>();
        public override IReadOnlyCollection<T> Children => Children_;

        public ViewStackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public override void Push(T child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Slot must have no {child} child" ).Valid( !Children_.Contains( child ) );
            BeforePush( VisualElement, Children_ );
            {
                Children_.Push( child );
                VisualElement.Add( child );
            }
        }
        public override T Peek() {
            Assert.Operation.Message( $"Slot must have child" ).Valid( Children_.Any() );
            var result = Children_.Peek();
            return result;
        }
        public override T Pop() {
            Assert.Operation.Message( $"Slot must have child" ).Valid( Children_.Any() );
            var result = Children_.Peek();
            {
                VisualElement.Remove( result );
                Children_.Pop();
            }
            AfterPop( VisualElement, Children_ );
            return result;
        }

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

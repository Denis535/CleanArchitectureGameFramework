#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ViewSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIViewBase {

        public T? View { get; private set; }

        public ViewSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Set(T view) {
            Assert.Argument.Message( $"Argument 'view' must be not null" ).NotNull( view != null );
            Assert.Operation.Message( $"Slot must have no view" ).Valid( View == null );
            View = view;
            VisualElement.Add( view );
        }
        public void Clear() {
            Assert.Operation.Message( $"Slot must have view" ).Valid( View != null );
            VisualElement.Remove( View );
            View = null;
        }
        public void Clear(T view) {
            Assert.Argument.Message( $"Argument 'view' must be not null" ).NotNull( view != null );
            Assert.Operation.Message( $"Slot must have view" ).Valid( View != null );
            Assert.Operation.Message( $"Slot must have {view} view" ).Valid( View == view );
            VisualElement.Remove( View );
            View = null;
        }

    }
    public class ViewListSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIViewBase {

        private List<T> Views_ { get; } = new List<T>();
        public IReadOnlyList<T> Views => Views_;

        public ViewListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T view) {
            Assert.Argument.Message( $"Argument 'view' must be not null" ).NotNull( view != null );
            Assert.Operation.Message( $"Slot must have no {view} view" ).Valid( !Views_.Contains( view ) );
            Views_.Add( view );
            VisualElement.Add( view );
        }
        public void Remove(T view) {
            Assert.Argument.Message( $"Argument 'view' must be not null" ).NotNull( view != null );
            Assert.Operation.Message( $"Slot must have view" ).Valid( Views_.Any() );
            Assert.Operation.Message( $"Slot must have {view} view" ).Valid( Views_.Contains( view ) );
            VisualElement.Remove( view );
            Views_.Remove( view );
        }
        public void Clear() {
            foreach (var view in Views_) {
                VisualElement.Remove( view );
            }
            Views_.Clear();
        }

    }
    public class ViewStackSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIViewBase {

        private Stack<T> Views_ { get; } = new Stack<T>();
        public IReadOnlyCollection<T> Views => Views_;

        public ViewStackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Push(T view) {
            Assert.Argument.Message( $"Argument 'view' must be not null" ).NotNull( view != null );
            Assert.Operation.Message( $"Slot must have no {view} view" ).Valid( !Views_.Contains( view ) );
            BeforePush( VisualElement, Views_ );
            {
                Views_.Push( view );
                VisualElement.Add( view );
            }
        }
        public T Peek() {
            Assert.Operation.Message( $"Slot must have view" ).Valid( Views_.Any() );
            var result = Views_.Peek();
            return result;
        }
        public T Pop() {
            Assert.Operation.Message( $"Slot must have view" ).Valid( Views_.Any() );
            var result = Views_.Peek();
            {
                VisualElement.Remove( result );
                Views_.Pop();
            }
            AfterPop( VisualElement, Views_ );
            return result;
        }

        // helpers
        private static void BeforePush(VisualElement element, Stack<T> views) {
            if (views.TryPeek( out var last )) {
                element.Remove( last );
            }
        }
        private static void AfterPop(VisualElement element, Stack<T> views) {
            if (views.TryPeek( out var last )) {
                element.Add( last );
            }
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class WidgetSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIWidgetBase {

        public T? Widget { get; private set; }

        public WidgetSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Set(T widget) {
            Assert.Argument.Message( $"Argument 'widget' must be not null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Slot must have no widget" ).Valid( Widget == null );
            Widget = widget;
            VisualElement.Add( widget );
        }
        public void Clear() {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widget != null );
            VisualElement.Remove( Widget );
            Widget = null;
        }
        public void Clear(T widget) {
            Assert.Argument.Message( $"Argument 'widget' must be not null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widget != null );
            Assert.Operation.Message( $"Slot must have {widget} widget" ).Valid( Widget == widget );
            VisualElement.Remove( Widget );
            Widget = null;
        }

    }
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
    // ListSlot
    public class WidgetListSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIWidgetBase {

        private List<T> Widgets_ { get; } = new List<T>();
        public IReadOnlyList<T> Widgets => Widgets_;

        public WidgetListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Add(T widget) {
            Assert.Argument.Message( $"Argument 'widget' must be not null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Slot must have no {widget} widget" ).Valid( !Widgets_.Contains( widget ) );
            Widgets_.Add( widget );
            VisualElement.Add( widget );
        }
        public void Remove(T widget) {
            Assert.Argument.Message( $"Argument 'widget' must be not null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widgets_.Any() );
            Assert.Operation.Message( $"Slot must have {widget} widget" ).Valid( Widgets_.Contains( widget ) );
            VisualElement.Remove( widget );
            Widgets_.Remove( widget );
        }
        public void Clear() {
            foreach (var widget in Widgets_) {
                VisualElement.Remove( widget );
            }
            Widgets_.Clear();
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
    // StackSlot
    public class WidgetStackSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull, UIWidgetBase {

        private Stack<T> Widgets_ { get; } = new Stack<T>();
        public IReadOnlyCollection<T> Widgets => Widgets_;

        public WidgetStackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public void Push(T widget) {
            Assert.Argument.Message( $"Argument 'widget' must be not null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Slot must have no {widget} widget" ).Valid( !Widgets_.Contains( widget ) );
            BeforePush( VisualElement, Widgets_ );
            {
                Widgets_.Push( widget );
                VisualElement.Add( widget );
            }
        }
        public T Peek() {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widgets_.Any() );
            var result = Widgets_.Peek();
            return result;
        }
        public T Pop() {
            Assert.Operation.Message( $"Slot must have widget" ).Valid( Widgets_.Any() );
            var result = Widgets_.Peek();
            {
                VisualElement.Remove( result );
                Widgets_.Pop();
            }
            AfterPop( VisualElement, Widgets_ );
            return result;
        }

        // helpers
        private static void BeforePush(VisualElement element, Stack<T> widgets) {
            if (widgets.TryPeek( out var last )) {
                element.Remove( last );
            }
        }
        private static void AfterPop(VisualElement element, Stack<T> widgets) {
            if (widgets.TryPeek( out var last )) {
                element.Add( last );
            }
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

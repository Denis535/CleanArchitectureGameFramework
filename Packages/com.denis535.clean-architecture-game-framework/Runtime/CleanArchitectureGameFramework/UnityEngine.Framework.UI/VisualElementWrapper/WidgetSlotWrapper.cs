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
}

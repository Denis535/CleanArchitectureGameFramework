#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    public static class VisualElementSlotWrapperExtensions {

        // AsSlot
        public static SlotWrapper<T> AsSlot<T>(this VisualElement visualElement) where T : notnull, VisualElement {
            return new SlotWrapper<T>( visualElement );
        }
        public static ListSlotWrapper<T> AsListSlot<T>(this VisualElement visualElement) where T : notnull, VisualElement {
            return new ListSlotWrapper<T>( visualElement );
        }
        public static StackSlotWrapper<T> AsStackSlot<T>(this VisualElement visualElement) where T : notnull, VisualElement {
            return new StackSlotWrapper<T>( visualElement );
        }

        // AsWidgetSlot
        public static WidgetSlotWrapper<T> AsWidgetSlot<T>(this VisualElement visualElement) where T : notnull, UIWidgetBase {
            return new WidgetSlotWrapper<T>( visualElement );
        }
        public static WidgetListSlotWrapper<T> AsWidgetListSlot<T>(this VisualElement visualElement) where T : notnull, UIWidgetBase {
            return new WidgetListSlotWrapper<T>( visualElement );
        }
        public static WidgetStackSlotWrapper<T> AsWidgetStackSlot<T>(this VisualElement visualElement) where T : notnull, UIWidgetBase {
            return new WidgetStackSlotWrapper<T>( visualElement );
        }

        // AsViewSlot
        public static ViewSlotWrapper<T> AsViewSlot<T>(this VisualElement visualElement) where T : notnull, UIViewBase {
            return new ViewSlotWrapper<T>( visualElement );
        }
        public static ViewListSlotWrapper<T> AsViewListSlot<T>(this VisualElement visualElement) where T : notnull, UIViewBase {
            return new ViewListSlotWrapper<T>( visualElement );
        }
        public static ViewStackSlotWrapper<T> AsViewStackSlot<T>(this VisualElement visualElement) where T : notnull, UIViewBase {
            return new ViewStackSlotWrapper<T>( visualElement );
        }

    }
}

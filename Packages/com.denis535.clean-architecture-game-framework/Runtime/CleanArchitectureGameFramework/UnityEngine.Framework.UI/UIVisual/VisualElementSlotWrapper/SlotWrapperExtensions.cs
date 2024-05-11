#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    public static class SlotWrapperExtensions {

        public static ElementSlotWrapper<T> AsElementSlot<T>(this VisualElement visualElement) where T : notnull, VisualElement {
            return new ElementSlotWrapper<T>( visualElement );
        }
        public static ElementListSlotWrapper<T> AsElementListSlot<T>(this VisualElement visualElement) where T : notnull, VisualElement {
            return new ElementListSlotWrapper<T>( visualElement );
        }
        public static ElementStackSlotWrapper<T> AsElementStackSlot<T>(this VisualElement visualElement) where T : notnull, VisualElement {
            return new ElementStackSlotWrapper<T>( visualElement );
        }

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

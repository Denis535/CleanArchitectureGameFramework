#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class VisualElementWrapperExtensions2 {

        // Wrap
        public static ElementWrapper Wrap(this VisualElement visualElement) {
            return new ElementWrapper( visualElement );
        }
        public static LabelWrapper Wrap(this Label visualElement) {
            return new LabelWrapper( visualElement );
        }
        public static ButtonWrapper Wrap(this Button visualElement) {
            return new ButtonWrapper( visualElement );
        }
        public static RepeatButtonWrapper Wrap(this RepeatButton visualElement) {
            return new RepeatButtonWrapper( visualElement );
        }
        public static ImageWrapper Wrap(this Image visualElement) {
            return new ImageWrapper( visualElement );
        }
        public static TextFieldWrapper<string> Wrap(this BaseField<string?> visualElement) {
            return new TextFieldWrapper<string>( visualElement );
        }
        public static PopupFieldWrapper<T> Wrap<T>(this PopupField<T?> visualElement) where T : notnull {
            return new PopupFieldWrapper<T>( visualElement );
        }
        public static PopupFieldWrapper<string> Wrap(this DropdownField visualElement) {
            return new PopupFieldWrapper<string>( visualElement );
        }
        public static SliderFieldWrapper<T> Wrap<T>(this BaseSlider<T> visualElement) where T : struct, IComparable<T> {
            return new SliderFieldWrapper<T>( visualElement );
        }
        public static ToggleFieldWrapper<bool> Wrap(this Toggle visualElement) {
            return new ToggleFieldWrapper<bool>( visualElement );
        }
        public static T Wrap<T>(this VisualElement visualElement) where T : VisualElementWrapper {
            return (T) Activator.CreateInstance( typeof( T ), visualElement );
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

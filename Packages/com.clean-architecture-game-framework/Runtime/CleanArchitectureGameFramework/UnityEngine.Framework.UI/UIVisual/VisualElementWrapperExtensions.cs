#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class VisualElementWrapperExtensions {

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

        // AsSlot
        public static SlotWrapper AsSlot(this VisualElement visualElement) {
            return new SlotWrapper( visualElement );
        }

        // OnEvent
        public static void OnEvent<T>(this VisualElementWrapper wrapper, EventCallback<T> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : EventBase<T>, new() {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnEvent<T, TArg>(this VisualElementWrapper wrapper, EventCallback<T, TArg> callback, TArg arg, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : EventBase<T>, new() {
            wrapper.VisualElement.RegisterCallback( callback, arg, useTrickleDown );
        }

        // OnAttachToPanel
        public static void OnAttachToPanel(this VisualElementWrapper wrapper, EventCallback<AttachToPanelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnDetachFromPanel(this VisualElementWrapper wrapper, EventCallback<DetachFromPanelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnGeometryChanged
        public static void OnGeometryChanged(this VisualElementWrapper wrapper, EventCallback<GeometryChangedEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnFocus
        public static void OnFocusIn(this VisualElementWrapper wrapper, EventCallback<FocusInEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately before an element gains focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnFocus(this VisualElementWrapper wrapper, EventCallback<FocusEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately after an element has gained focus. This event trickles down (and does not bubbles up).
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnFocusOut(this VisualElementWrapper wrapper, EventCallback<FocusOutEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately before an element loses focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnClick
        public static void OnClick(this VisualElementWrapper wrapper, EventCallback<ClickEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnChange
        public static void OnChange<T>(this VisualElementWrapper wrapper, EventCallback<ChangeEvent<T?>> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : notnull {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnChange<T>(this IVisualElementWrapper<BaseField<T?>> wrapper, EventCallback<ChangeEvent<T?>> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : notnull {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnChangeAny
        public static void OnChangeAny(this VisualElementWrapper wrapper, EventCallback<IChangeEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<string?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<object?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<int?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<float?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<bool?>>( callback, useTrickleDown );
        }

        // OnSubmit
        public static void OnSubmit(this VisualElementWrapper wrapper, EventCallback<NavigationSubmitEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnCancel(this VisualElementWrapper wrapper, EventCallback<NavigationCancelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // GetVisualElement
        public static VisualElement GetVisualElement(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement;
        }
        public static T GetVisualElement<T>(this VisualElementWrapper<T> wrapper) where T : VisualElement {
            return wrapper.VisualElement;
        }

    }
}

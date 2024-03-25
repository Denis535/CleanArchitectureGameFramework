#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static partial class VisualElementWrapperExtensions {

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
        public static SlotWrapper<TView> AsSlot<TView>(this VisualElement visualElement) where TView : UIViewBase {
            return new SlotWrapper<TView>( visualElement );
        }

    }
    public static partial class VisualElementWrapperExtensions {

        // IsEnabled
        public static bool IsEnabledInHierarchy(this IVisualElementWrapper<VisualElement> wrapper) {
            return wrapper.VisualElement.enabledInHierarchy;
        }
        public static bool IsEnabledSelf(this IVisualElementWrapper<VisualElement> wrapper) {
            return wrapper.VisualElement.enabledSelf;
        }
        public static void SetEnabled(this IVisualElementWrapper<VisualElement> wrapper, bool value) {
            wrapper.VisualElement.SetEnabled( value );
        }

        // IsDisplayed
        public static bool IsDisplayed(this IVisualElementWrapper<VisualElement> wrapper) {
            return wrapper.VisualElement.IsDisplayed();
        }
        public static void SetDisplayed(this IVisualElementWrapper<VisualElement> wrapper, bool value) {
            wrapper.VisualElement.SetDisplayed( value );
        }

        // IsValid
        public static bool IsValid(this IVisualElementWrapper<VisualElement> wrapper) {
            return wrapper.VisualElement.IsValid();
        }
        public static void SetValid(this IVisualElementWrapper<VisualElement> wrapper, bool value) {
            wrapper.VisualElement.SetValid( value );
        }

        // GetClasses
        public static IEnumerable<string> GetClasses(this IVisualElementWrapper<VisualElement> wrapper) {
            return wrapper.VisualElement.GetClasses();
        }
        public static void AddClass(this IVisualElementWrapper<VisualElement> wrapper, string @class) {
            wrapper.VisualElement.AddToClassList( @class );
        }
        public static void RemoveClass(this IVisualElementWrapper<VisualElement> wrapper, string @class) {
            wrapper.VisualElement.RemoveFromClassList( @class );
        }
        public static void ToggleClass(this IVisualElementWrapper<VisualElement> wrapper, string @class) {
            wrapper.VisualElement.ToggleInClassList( @class );
        }
        public static void EnableClass(this IVisualElementWrapper<VisualElement> wrapper, string @class, bool isEnabled) {
            wrapper.VisualElement.EnableInClassList( @class, isEnabled );
        }
        public static bool ContainsClass(this IVisualElementWrapper<VisualElement> wrapper, string @class) {
            return wrapper.VisualElement.ClassListContains( @class );
        }
        public static void ClearClasses(this IVisualElementWrapper<VisualElement> wrapper) {
            wrapper.VisualElement.ClearClassList();
        }

        // OnEvent
        public static void OnEvent<T>(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<T> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : EventBase<T>, new() {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnEvent<T, TArg>(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<T, TArg> callback, TArg arg, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : EventBase<T>, new() {
            wrapper.VisualElement.RegisterCallback( callback, arg, useTrickleDown );
        }

        // OnAttachToPanel
        public static void OnAttachToPanel(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<AttachToPanelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnDetachFromPanel(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<DetachFromPanelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnGeometryChanged
        public static void OnGeometryChanged(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<GeometryChangedEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnFocus
        public static void OnFocusIn(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<FocusInEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately before an element gains focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnFocus(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<FocusEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately after an element has gained focus. This event trickles down (and does not bubbles up).
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnFocusOut(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<FocusOutEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately before an element loses focus. This event trickles down and bubbles up.
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnClick
        public static void OnClick(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<ClickEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnChange
        public static void OnChange<T>(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<ChangeEvent<T?>> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : notnull {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnChange<T>(this IVisualElementWrapper<BaseField<T?>> wrapper, EventCallback<ChangeEvent<T?>> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : notnull {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // OnChangeAny
        public static void OnChangeAny(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<IChangeEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback<ChangeEvent<string?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<object?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<int?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<float?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<bool?>>( callback, useTrickleDown );
        }

        // OnSubmit
        public static void OnSubmit(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<NavigationSubmitEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnCancel(this IVisualElementWrapper<VisualElement> wrapper, EventCallback<NavigationCancelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            wrapper.VisualElement.RegisterCallback( callback, useTrickleDown );
        }

        // GetVisualElement
        public static VisualElement __GetVisualElement__(this IVisualElementWrapper<VisualElement> wrapper) {
            // try not to use it
            return wrapper.VisualElement;
        }
        public static T __GetVisualElement__<T>(this IVisualElementWrapper<T> wrapper) where T : VisualElement {
            // try not to use it
            return wrapper.VisualElement;
        }

    }
}

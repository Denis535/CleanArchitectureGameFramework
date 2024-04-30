#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class VisualElementWrapperExtensions2 {

        // IsEnabled
        public static bool IsEnabledInHierarchy(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement.enabledInHierarchy;
        }
        public static bool IsEnabledSelf(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement.enabledSelf;
        }
        public static void SetEnabled(this VisualElementWrapper wrapper, bool value) {
            wrapper.VisualElement.SetEnabled( value );
        }

        // IsDisplayed
        public static bool IsDisplayed(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement.IsDisplayedSelf();
        }
        public static void SetDisplayed(this VisualElementWrapper wrapper, bool value) {
            wrapper.VisualElement.SetDisplayed( value );
        }

        // IsValid
        public static bool IsValid(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement.IsValidSelf();
        }
        public static void SetValid(this VisualElementWrapper wrapper, bool value) {
            wrapper.VisualElement.SetValid( value );
        }

        // GetClasses
        public static IEnumerable<string> GetClasses(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement.GetClasses();
        }
        public static void AddClass(this VisualElementWrapper wrapper, string @class) {
            wrapper.VisualElement.AddToClassList( @class );
        }
        public static void RemoveClass(this VisualElementWrapper wrapper, string @class) {
            wrapper.VisualElement.RemoveFromClassList( @class );
        }
        public static void ToggleClass(this VisualElementWrapper wrapper, string @class) {
            wrapper.VisualElement.ToggleInClassList( @class );
        }
        public static void EnableClass(this VisualElementWrapper wrapper, string @class, bool isEnabled) {
            wrapper.VisualElement.EnableInClassList( @class, isEnabled );
        }
        public static bool ContainsClass(this VisualElementWrapper wrapper, string @class) {
            return wrapper.VisualElement.ClassListContains( @class );
        }
        public static void ClearClasses(this VisualElementWrapper wrapper) {
            wrapper.VisualElement.ClearClassList();
        }

        // GetStyle
        public static IStyle GetStyle(this VisualElementWrapper wrapper) {
            return wrapper.VisualElement.style;
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
            wrapper.VisualElement.RegisterCallback<ChangeEvent<object?>>( callback, useTrickleDown );
            wrapper.VisualElement.RegisterCallback<ChangeEvent<string?>>( callback, useTrickleDown );
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
        public static VisualElement __GetVisualElement__(this VisualElementWrapper wrapper) {
            // try not to use it
            return wrapper.VisualElement;
        }
        public static T __GetVisualElement__<T>(this VisualElementWrapper<T> wrapper) where T : VisualElement {
            // try not to use it
            return wrapper.VisualElement;
        }

    }
}

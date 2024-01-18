#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static partial class VisualElementExtensions {

        // IsAttached
        public static bool IsAttached(this VisualElement element) {
            return element.panel != null;
        }

        // IsDisplayed
        public static bool IsDisplayed(this VisualElement element) {
            return element.style.display == DisplayStyle.Flex;
        }
        public static void SetDisplayed(this VisualElement element, bool value) {
            if (value) {
                element.style.display = DisplayStyle.Flex;
            } else {
                element.style.display = DisplayStyle.None;
            }
        }

        // IsValid
        public static bool IsValid(this VisualElement element) {
            return !element.ClassListContains( "invalid" );
        }
        public static void SetValid(this VisualElement element, bool value) {
            if (value) {
                element.RemoveFromClassList( "invalid" );
            } else {
                element.AddToClassList( "invalid" );
            }
        }

        // SetUp
        public static T Name<T>(this T element, string? name) where T : VisualElement {
            element.name = name;
            return element;
        }
        public static T Classes<T>(this T element, params string?[] classes) where T : VisualElement {
            foreach (var @class in classes) {
                element.AddToClassList( @class );
            }
            return element;
        }
        public static T Style<T>(this T element, Action<IStyle> callback) where T : VisualElement {
            callback( element.style );
            return element;
        }

        // SetUp
        public static T Text<T>(this T element, string? text) where T : TextElement {
            element.text = text;
            return element;
        }
        public static T UserData<T>(this T element, object? userData) where T : VisualElement {
            element.userData = userData;
            return element;
        }

        // SetUp
        public static T Children<T>(this T element, params VisualElement?[] children) where T : VisualElement {
            foreach (var child in children) {
                element.Add( child );
            }
            return element;
        }

        // FindElement
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T FindElement<T>(this VisualElement element, string? name, params string[] classes) where T : VisualElement {
            return element.Query<T>( name, classes ).First();
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] FindElements<T>(this VisualElement element, string? name, params string[] classes) where T : VisualElement {
            return element.Query<T>( name, classes ).ToList().ToArray();
        }

        // RequireElement
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireElement<T>(this VisualElement element, string? name, params string[] classes) where T : VisualElement {
            var result = element.Query<T>( name, classes ).First();
            Assert.Operation.Message( $"Element {typeof( T )} ({name}, {classes}) was not found" ).Valid( result != null );
            return result;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireElements<T>(this VisualElement element, string? name, params string[] classes) where T : VisualElement {
            var result = element.Query<T>( name, classes ).ToList().ToArray().NullIfEmpty();
            Assert.Operation.Message( $"Elements {typeof( T )} ({name}, {classes}) was not found" ).Valid( result != null );
            return result;
        }

        // OnEvent/TrickleDown
        public static void OnEventTrickleDown<TEvt>(this VisualElement element, EventCallback<TEvt> callback) where TEvt : EventBase<TEvt>, new() {
            element.RegisterCallback( callback, TrickleDown.TrickleDown );
        }
        public static void OnEventTrickleDown<TEvt, TArg>(this VisualElement element, EventCallback<TEvt, TArg> callback, TArg arg) where TEvt : EventBase<TEvt>, new() {
            element.RegisterCallback( callback, arg, TrickleDown.TrickleDown );
        }

        // OnEvent/BubbleUp
        public static void OnEvent<TEvt>(this VisualElement element, EventCallback<TEvt> callback) where TEvt : EventBase<TEvt>, new() {
            element.RegisterCallback( callback, TrickleDown.NoTrickleDown );
        }
        public static void OnEvent<TEvt, TArg>(this VisualElement element, EventCallback<TEvt, TArg> callback, TArg arg) where TEvt : EventBase<TEvt>, new() {
            element.RegisterCallback( callback, arg, TrickleDown.NoTrickleDown );
        }

        // OnAttachToPanel
        public static void OnAttachToPanel(this VisualElement element, EventCallback<AttachToPanelEvent> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnDetachFromPanel(this VisualElement element, EventCallback<DetachFromPanelEvent> callback) {
            element.RegisterCallback( callback );
        }

        // OnFocus
        public static void OnFocusIn(this VisualElement element, EventCallback<FocusInEvent> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnFocus(this VisualElement element, EventCallback<FocusEvent> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnFocusOut(this VisualElement element, EventCallback<FocusOutEvent> callback) {
            element.RegisterCallback( callback );
        }

        // OnMouse
        public static void OnMouseOver(this VisualElement element, EventCallback<MouseOverEvent> callback) {
            element.RegisterCallback( callback ); // Event sent when the mouse pointer enters an element.
        }
        public static void OnMouseOut(this VisualElement element, EventCallback<MouseOutEvent> callback) {
            element.RegisterCallback( callback ); // Event sent when the mouse pointer exits an element.
        }

        // OnMouse
        public static void OnMouseEnter(this VisualElement element, EventCallback<MouseEnterEvent> callback) {
            element.RegisterCallback( callback ); // Event sent when the mouse pointer enters an element or one of its descendent elements.
        }
        public static void OnMouseLeave(this VisualElement element, EventCallback<MouseLeaveEvent> callback) {
            element.RegisterCallback( callback ); // Event sent when the mouse pointer exits an element and all its descendent elements.
        }

        // OnMouse
        public static void OnMouseDown(this VisualElement element, EventCallback<MouseDownEvent> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnMouseUp(this VisualElement element, EventCallback<MouseUpEvent> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnMouseMove(this VisualElement element, EventCallback<MouseMoveEvent> callback) {
            element.RegisterCallback( callback );
        }

        // OnClick
        public static void OnClick(this VisualElement element, EventCallback<ClickEvent> callback) {
            element.RegisterCallback( callback );
        }

        // OnChange
        public static void OnChange<T>(this VisualElement element, EventCallback<ChangeEvent<T>> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnChange<T>(this BaseField<T> element, EventCallback<ChangeEvent<T>> callback) {
            element.RegisterCallback( callback );
        }

        // OnSubmit
        public static void OnSubmit(this VisualElement element, EventCallback<NavigationSubmitEvent> callback) {
            element.RegisterCallback( callback );
        }
        public static void OnCancel(this VisualElement element, EventCallback<NavigationCancelEvent> callback) {
            element.RegisterCallback( callback );
        }

    }
}

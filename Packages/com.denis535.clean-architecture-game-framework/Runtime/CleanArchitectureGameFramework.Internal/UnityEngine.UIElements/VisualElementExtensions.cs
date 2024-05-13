#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static partial class VisualElementExtensions {

        // IsAttached
        public static bool IsAttached(this VisualElement element) {
            return element.panel != null;
        }

        // IsDisplayed
        public static bool IsDisplayedSelf(this VisualElement element) {
            return element.resolvedStyle.display == DisplayStyle.Flex;
        }
        public static bool IsDisplayedInHierarchy(this VisualElement element) {
            return element.GetAncestorsAndSelf().All( IsDisplayedSelf );
        }
        public static void SetDisplayed(this VisualElement element, bool value) {
            if (value) {
                element.style.display = DisplayStyle.Flex;
            } else {
                element.style.display = DisplayStyle.None;
            }
        }

        // IsValid
        public static bool IsValidSelf(this VisualElement element) {
            return !element.ClassListContains( "invalid" );
        }
        public static bool IsValidInHierarchy(this VisualElement element) {
            return element.GetAncestorsAndSelf().All( IsValidSelf );
        }
        public static void SetValid(this VisualElement element, bool value) {
            if (value) {
                element.RemoveFromClassList( "invalid" );
            } else {
                element.AddToClassList( "invalid" );
            }
        }

        // IsContentValid
        public static bool IsContentValid(this VisualElement element) {
            return element.GetDescendants().All( IsValidSelf );
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
        public static T Text<T>(this T element, string? text) where T : TextElement {
            element.text = text;
            return element;
        }
        public static T UserData<T>(this T element, object? userData) where T : VisualElement {
            element.userData = userData;
            return element;
        }
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

        // SendEvent
        public static void SendEventImmediate(this VisualElement element, EventBase @event) {
            Assert.Operation.Message( $"Element {element} must be attached" ).Valid( element.IsAttached() );
            var type_VisualElement = typeof( VisualElement );
            var type_EventBase = typeof( EventBase );
            var type_DispatchMode = typeof( VisualElement ).Assembly.GetType( "UnityEngine.UIElements.DispatchMode" ) ?? throw Exceptions.Internal.Exception( $"Can not find 'DispatchMode' type" );
            var method_SendEvent = type_VisualElement.GetMethod( "SendEvent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, new Type[] { type_EventBase, type_DispatchMode }, null ) ?? throw Exceptions.Internal.Exception( $"Can not find 'SendEvent' method" );
            method_SendEvent.Invoke( element, new object[] { @event, 2 } );
        }

        // OnEvent
        public static void OnEvent<T>(this VisualElement element, EventCallback<T> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : EventBase<T>, new() {
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnEvent<T, TArg>(this VisualElement element, EventCallback<T, TArg> callback, TArg arg, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where T : EventBase<T>, new() {
            element.RegisterCallback( callback, arg, useTrickleDown );
        }

        // OnAttachToPanel
        public static void OnAttachToPanel(this VisualElement element, EventCallback<AttachToPanelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnDetachFromPanel(this VisualElement element, EventCallback<DetachFromPanelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnGeometryChanged
        public static void OnGeometryChanged(this VisualElement element, EventCallback<GeometryChangedEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnFocus
        public static void OnFocusIn(this VisualElement element, EventCallback<FocusInEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately before an element gains focus. This event trickles down and bubbles up.
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnFocus(this VisualElement element, EventCallback<FocusEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately after an element has gained focus. This event trickles down (and does not bubbles up).
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnFocusOut(this VisualElement element, EventCallback<FocusOutEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent immediately before an element loses focus. This event trickles down and bubbles up.
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnMouse
        public static void OnMouseOver(this VisualElement element, EventCallback<MouseOverEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent when the mouse pointer enters an element.
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnMouseOut(this VisualElement element, EventCallback<MouseOutEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent when the mouse pointer exits an element.
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnMouse
        public static void OnMouseEnter(this VisualElement element, EventCallback<MouseEnterEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent when the mouse pointer enters an element or one of its descendent elements.
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnMouseLeave(this VisualElement element, EventCallback<MouseLeaveEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            // Event sent when the mouse pointer exits an element and all its descendent elements.
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnMouse
        public static void OnMouseDown(this VisualElement element, EventCallback<MouseDownEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnMouseUp(this VisualElement element, EventCallback<MouseUpEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnMouseMove(this VisualElement element, EventCallback<MouseMoveEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnClick
        public static void OnClick(this VisualElement element, EventCallback<ClickEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnChange
        public static void OnChange<T>(this VisualElement element, EventCallback<ChangeEvent<T>> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnChange<T>(this BaseField<T> element, EventCallback<ChangeEvent<T>> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }

        // OnChangeAny
        public static void OnChangeAny(this VisualElement element, EventCallback<IChangeEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback<ChangeEvent<object>>( callback, useTrickleDown );
            element.RegisterCallback<ChangeEvent<string>>( callback, useTrickleDown );
            element.RegisterCallback<ChangeEvent<int>>( callback, useTrickleDown );
            element.RegisterCallback<ChangeEvent<float>>( callback, useTrickleDown );
            element.RegisterCallback<ChangeEvent<bool>>( callback, useTrickleDown );
        }

        // OnSubmit
        public static void OnSubmit(this VisualElement element, EventCallback<NavigationSubmitEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }
        public static void OnCancel(this VisualElement element, EventCallback<NavigationCancelEvent> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) {
            element.RegisterCallback( callback, useTrickleDown );
        }

    }
}

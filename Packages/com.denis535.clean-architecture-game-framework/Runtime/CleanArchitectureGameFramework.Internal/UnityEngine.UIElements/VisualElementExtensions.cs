#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
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

    }
}

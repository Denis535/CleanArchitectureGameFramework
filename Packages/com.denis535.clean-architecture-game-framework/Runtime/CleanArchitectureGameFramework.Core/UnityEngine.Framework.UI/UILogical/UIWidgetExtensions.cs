#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class UIWidgetExtensions {

        // GetAncestors
        public static IEnumerable<UIWidgetBase> GetAncestors(this UIWidgetBase widget) {
            while (widget.Parent != null) {
                yield return widget.Parent;
                widget = widget.Parent;
            }
        }
        public static IEnumerable<UIWidgetBase> GetAncestorsAndSelf(this UIWidgetBase widget) {
            yield return widget;
            foreach (var i in GetAncestors( widget )) yield return i;
        }

        // GetDescendants
        public static IEnumerable<UIWidgetBase> GetDescendants(this UIWidgetBase widget) {
            foreach (var child in widget.Children) {
                yield return child;
                foreach (var i in GetDescendants( child )) yield return i;
            }
        }
        public static IEnumerable<UIWidgetBase> GetDescendantsAndSelf(this UIWidgetBase widget) {
            yield return widget;
            foreach (var i in GetDescendants( widget )) yield return i;
        }

        // OnAttach
        public static void OnBeforeAttach(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBeforeAttachEvent += callback;
        }
        public static void OnAfterAttach(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfterAttachEvent += callback;
        }
        public static void OnBeforeDetach(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBeforeDetachEvent += callback;
        }
        public static void OnAfterDetach(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfterDetachEvent += callback;
        }

        // OnDescendantAttach
        public static void OnBeforeDescendantAttach(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBeforeDescendantAttachEvent += callback;
        }
        public static void OnAfterDescendantAttach(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfterDescendantAttachEvent += callback;
        }
        public static void OnBeforeDescendantDetach(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBeforeDescendantDetachEvent += callback;
        }
        public static void OnAfterDescendantDetach(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfterDescendantDetachEvent += callback;
        }

        // AttachChild
        //public static void AttachChild(this UIWidgetBase widget, UIWidgetBase child, object? argument = null) {
        //    Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
        //    Assert.Operation.Message( $"Widget {widget} must have no child {child} widget" ).Valid( !widget.Children.Contains( child ) );
        //    widget.AttachChild( child, argument );
        //}

        // DetachSelf
        public static void DetachSelf(this UIWidgetBase widget, object? argument = null) {
            Assert.Operation.Message( $"Widget {widget} must have parent or must be attached" ).Valid( widget.Parent != null || widget.IsAttached );
            if (widget.Parent != null) {
                DetachChild( widget.Parent, widget, argument );
            } else {
                UIScreenExtensions.DetachWidget( widget.Screen!, widget, argument );
            }
        }

        // DetachChild
        public static void DetachChild<T>(this UIWidgetBase widget, object? argument = null) where T : UIWidgetBase {
            Assert.Operation.Message( $"Widget {widget} must have child {typeof( T )} widget" ).Valid( widget.Children.OfType<T>().Any() );
            widget.DetachChild( widget.Children.OfType<T>().Last(), argument );
        }
        public static void DetachChild(this UIWidgetBase widget, UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Widget {widget} must have child {child} widget" ).Valid( widget.Children.Contains( child ) );
            widget.DetachChild( child, argument );
        }

        // DetachChildren
        public static void DetachChildren(this UIWidgetBase widget, object? argument = null) {
            foreach (var child in widget.Children.Reverse()) {
                widget.DetachChild( child, argument );
            }
        }

        // AttachToScreen
        internal static void AttachToScreen(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be non-attached" ).Valid( widget.IsNonAttached );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen == null );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen is not null );
            widget.OnBeforeAttach( argument );
            widget.State = UIWidgetState.Attaching;
            widget.Screen = screen;
            {
                widget.OnAttach( argument );
                foreach (var child in widget.Children) {
                    AttachToScreen( child, screen, argument );
                }
            }
            widget.State = UIWidgetState.Attached;
            widget.OnAfterAttach( argument );
        }
        internal static void DetachFromScreen(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be attached" ).Valid( widget.IsAttached );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen == screen );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen is not null );
            widget.OnBeforeDetach( argument );
            widget.State = UIWidgetState.Detaching;
            {
                foreach (var child in widget.Children.Reverse()) {
                    DetachFromScreen( child, screen, argument );
                }
                widget.OnDetach( argument );
            }
            widget.Screen = null;
            widget.State = UIWidgetState.Detached;
            widget.OnAfterDetach( argument );
        }

    }
}

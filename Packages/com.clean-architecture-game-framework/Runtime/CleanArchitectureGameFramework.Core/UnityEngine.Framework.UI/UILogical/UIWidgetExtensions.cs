#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class UIWidgetExtensions {

        // AttachChild
        public static void AttachChild(this UIWidgetBase widget, UIWidgetBase child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Object.Message( $"Widget {widget} must have no child {child} widget" ).Valid( !widget.Children.Contains( child ) );
            widget.__AttachChild__( child );
        }

        // DetachSelf
        public static void DetachSelf(this UIWidgetBase widget) {
            Assert.Object.Message( $"Widget {widget} must have parent or must be attached" ).Valid( widget.Parent != null || widget.IsAttached );
            if (widget.Parent != null) {
                widget.Parent.DetachChild( widget );
            } else {
                widget.Screen!.DetachWidget( widget );
            }
        }

        // DetachChild
        public static void DetachChild<T>(this UIWidgetBase widget) where T : UIWidgetBase {
            Assert.Object.Message( $"Widget {widget} must have child {typeof( T )} widget" ).Valid( widget.Children.OfType<T>().Any() );
            widget.__DetachChild__( widget.Children.OfType<T>().Last() );
        }
        public static void DetachChild(this UIWidgetBase widget, UIWidgetBase child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Object.Message( $"Widget {widget} must have child {child} widget" ).Valid( widget.Children.Contains( child ) );
            widget.__DetachChild__( child );
        }

        // DetachChildren
        public static void DetachChildren(this UIWidgetBase widget) {
            foreach (var child in widget.Children.Reverse()) {
                widget.__DetachChild__( child );
            }
        }

        // OnAttach
        public static void OnBeforeAttach(this UIWidgetBase widget, Action? callback) {
            widget.OnBeforeAttachEvent += callback;
        }
        public static void OnAfterAttach(this UIWidgetBase widget, Action? callback) {
            widget.OnAfterAttachEvent += callback;
        }
        public static void OnBeforeDetach(this UIWidgetBase widget, Action? callback) {
            widget.OnBeforeDetachEvent += callback;
        }
        public static void OnAfterDetach(this UIWidgetBase widget, Action? callback) {
            widget.OnAfterDetachEvent += callback;
        }

        // OnDescendantAttach
        public static void OnBeforeDescendantAttach(this UIWidgetBase widget, Action<UIWidgetBase>? callback) {
            widget.OnBeforeDescendantAttachEvent += callback;
        }
        public static void OnAfterDescendantAttach(this UIWidgetBase widget, Action<UIWidgetBase>? callback) {
            widget.OnAfterDescendantAttachEvent += callback;
        }
        public static void OnBeforeDescendantDetach(this UIWidgetBase widget, Action<UIWidgetBase>? callback) {
            widget.OnBeforeDescendantDetachEvent += callback;
        }
        public static void OnAfterDescendantDetach(this UIWidgetBase widget, Action<UIWidgetBase>? callback) {
            widget.OnAfterDescendantDetachEvent += callback;
        }

        // GetView
        public static UIViewBase? GetView(this UIWidgetBase widget) {
            return widget?.View;
        }

        // GetVisualElement
        public static VisualElement? GetVisualElement(this UIWidgetBase widget) {
            return widget?.View?.VisualElement;
        }

    }
}

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

        // AddChildInternal
        internal static void AddChildInternal(this UIWidgetBase widget, UIWidgetBase child, object? argument) {
            using (widget.@lock.Enter()) {
                widget.Children_.Add( child );
                child.Parent = widget;
                if (widget.State is UIWidgetState.Actived) {
                    child.Activate( widget.Screen!, argument );
                } else {
                    Assert.Operation.Message( $"Argument {argument} must be null" ).Valid( argument == null );
                }
            }
        }
        internal static void RemoveChildInternal(this UIWidgetBase widget, UIWidgetBase child, object? argument) {
            using (widget.@lock.Enter()) {
                if (widget.State is UIWidgetState.Actived) {
                    child.Deactivate( widget.Screen!, argument );
                } else {
                    Assert.Operation.Message( $"Argument {argument} must be null" ).Valid( argument == null );
                }
                child.Parent = null;
                widget.Children_.Remove( child );
            }
            if (child.DisposeWhenDetach) {
                child.Dispose();
            }
        }

        // Activate
        internal static void Activate(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            widget.OnBeforeAttach( argument );
            widget.State = UIWidgetState.Activating;
            widget.Screen = screen;
            {
                widget.OnAttach( argument );
                foreach (var child in widget.Children) {
                    child.Activate( screen, argument );
                }
            }
            widget.State = UIWidgetState.Actived;
            widget.OnAfterAttach( argument );
        }
        internal static void Deactivate(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            widget.OnBeforeDetach( argument );
            widget.State = UIWidgetState.Deactivating;
            {
                foreach (var child in widget.Children.Reverse()) {
                    child.Deactivate( screen, argument );
                }
                widget.OnDetach( argument );
            }
            widget.Screen = null;
            widget.State = UIWidgetState.Inactive;
            widget.OnAfterDetach( argument );
        }

    }
}

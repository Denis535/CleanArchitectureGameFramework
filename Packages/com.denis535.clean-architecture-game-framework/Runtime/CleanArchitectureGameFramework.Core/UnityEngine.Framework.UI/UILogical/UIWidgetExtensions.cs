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

        // OnActivate
        public static void OnBeforeActivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBeforeActivateEvent += callback;
        }
        public static void OnAfterActivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfterActivateEvent += callback;
        }
        public static void OnBeforeDeactivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBeforeDeactivateEvent += callback;
        }
        public static void OnAfterDeactivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfterDeactivateEvent += callback;
        }

        // OnDescendantActivate
        public static void OnBeforeDescendantActivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBeforeDescendantActivateEvent += callback;
        }
        public static void OnAfterDescendantActivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfterDescendantActivateEvent += callback;
        }
        public static void OnBeforeDescendantDeactivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBeforeDescendantDeactivateEvent += callback;
        }
        public static void OnAfterDescendantDeactivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfterDescendantDeactivateEvent += callback;
        }

        // AddChildInternal
        internal static void AddChildInternal(this UIWidgetBase widget, UIWidgetBase child, object? argument) {
            if (widget.State is UIWidgetState.Actived) {
                using (widget.@lock.Enter()) {
                    widget.Children_.Add( child );
                    child.Parent = widget;
                    child.Activate( widget.Screen!, argument );
                }
            } else {
                using (widget.@lock.Enter()) {
                    widget.Children_.Add( child );
                    child.Parent = widget;
                    Assert.Operation.Message( $"Argument {argument} must be null" ).Valid( argument == null );
                }
            }
        }
        internal static void RemoveChildInternal(this UIWidgetBase widget, UIWidgetBase child, object? argument) {
            if (widget.State is UIWidgetState.Actived) {
                using (widget.@lock.Enter()) {
                    child.Deactivate( widget.Screen!, argument );
                    child.Parent = null;
                    widget.Children_.Remove( child );
                }
            } else {
                using (widget.@lock.Enter()) {
                    Assert.Operation.Message( $"Argument {argument} must be null" ).Valid( argument == null );
                    child.Parent = null;
                    widget.Children_.Remove( child );
                }
            }
        }

        // Activate
        internal static void Activate(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            foreach (var ancestor in widget.GetAncestors().Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( widget, argument );
                ancestor.OnBeforeDescendantActivate( widget, argument );
            }
            {
                widget.OnBeforeActivateEvent?.Invoke( argument );
                widget.OnBeforeActivate( argument );
                {
                    widget.State = UIWidgetState.Activating;
                    widget.Screen = screen;
                    widget.OnActivate( argument );
                    foreach (var child in widget.Children) {
                        child.Activate( screen, argument );
                    }
                    widget.State = UIWidgetState.Actived;
                }
                widget.OnAfterActivate( argument );
                widget.OnAfterActivateEvent?.Invoke( argument );
            }
            foreach (var ancestor in widget.GetAncestors()) {
                ancestor.OnAfterDescendantActivate( widget, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( widget, argument );
            }
        }
        internal static void Deactivate(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            foreach (var ancestor in widget.GetAncestors().Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( widget, argument );
                ancestor.OnBeforeDescendantDeactivate( widget, argument );
            }
            {
                widget.OnBeforeDeactivateEvent?.Invoke( argument );
                widget.OnBeforeDeactivate( argument );
                {
                    widget.State = UIWidgetState.Deactivating;
                    foreach (var child in widget.Children.Reverse()) {
                        child.Deactivate( screen, argument );
                    }
                    widget.OnDeactivate( argument );
                    widget.Screen = null;
                    widget.State = UIWidgetState.Inactive;
                }
                widget.OnAfterDeactivate( argument );
                widget.OnAfterDeactivateEvent?.Invoke( argument );
            }
            foreach (var ancestor in widget.GetAncestors()) {
                ancestor.OnAfterDescendantDeactivate( widget, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( widget, argument );
            }
            if (widget.DisposeWhenDeactivate) {
                widget.Dispose();
            }
        }

    }
}

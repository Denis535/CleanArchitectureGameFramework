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
        public static void OnBefore_Activate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBefore_ActivateEvent += callback;
        }
        public static void OnAfter_Activate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfter_ActivateEvent += callback;
        }
        public static void OnBefore_Deactivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnBefore_DeactivateEvent += callback;
        }
        public static void OnAfter_Deactivate(this UIWidgetBase widget, Action<object?>? callback) {
            widget.OnAfter_DeactivateEvent += callback;
        }

        // OnDescendantActivate
        public static void OnBefore_DescendantActivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBefore_DescendantActivateEvent += callback;
        }
        public static void OnAfter_DescendantActivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfter_DescendantActivateEvent += callback;
        }
        public static void OnBefore_DescendantDeactivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnBefore_DescendantDeactivateEvent += callback;
        }
        public static void OnAfter_DescendantDeactivate(this UIWidgetBase widget, Action<UIWidgetBase, object?>? callback) {
            widget.OnAfter_DescendantDeactivateEvent += callback;
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
        }

        // Activate
        internal static void Activate(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            widget.Parent?.OnBefore_DescendantActivateEvent?.Invoke( widget, argument );
            widget.Parent?.OnBefore_DescendantActivate( widget, argument );
            {
                widget.OnBefore_ActivateEvent?.Invoke( argument );
                widget.OnBefore_Activate( argument );
                {
                    widget.State = UIWidgetState.Activating;
                    widget.Screen = screen;
                    widget.OnActivate( argument );
                    foreach (var child in widget.Children) {
                        child.Activate( screen, argument );
                    }
                    widget.State = UIWidgetState.Actived;
                }
                widget.OnAfter_Activate( argument );
                widget.OnAfter_ActivateEvent?.Invoke( argument );
            }
            widget.Parent?.OnAfter_DescendantActivate( widget, argument );
            widget.Parent?.OnAfter_DescendantActivateEvent?.Invoke( widget, argument );
        }
        internal static void Deactivate(this UIWidgetBase widget, UIScreenBase screen, object? argument) {
            widget.Parent?.OnBefore_DescendantDeactivateEvent?.Invoke( widget, argument );
            widget.Parent?.OnBefore_DescendantDeactivate( widget, argument );
            {
                widget.OnBefore_DeactivateEvent?.Invoke( argument );
                widget.OnBefore_Deactivate( argument );
                {
                    widget.State = UIWidgetState.Deactivating;
                    foreach (var child in widget.Children.Reverse()) {
                        child.Deactivate( screen, argument );
                    }
                    widget.OnDeactivate( argument );
                    widget.Screen = null;
                    widget.State = UIWidgetState.Inactive;
                }
                widget.OnAfter_Deactivate( argument );
                widget.OnAfter_DeactivateEvent?.Invoke( argument );
            }
            widget.Parent?.OnAfter_DescendantDeactivate( widget, argument );
            widget.Parent?.OnAfter_DescendantDeactivateEvent?.Invoke( widget, argument );
            if (widget.DisposeWhenDeactivate) {
                widget.Dispose();
            }
        }

    }
}

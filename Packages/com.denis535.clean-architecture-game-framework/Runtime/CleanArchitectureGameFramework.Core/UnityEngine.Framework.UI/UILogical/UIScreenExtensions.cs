#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class UIScreenExtensions {

        // AddWidgetInternal
        internal static void AddWidgetInternal(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            using (screen.@lock.Enter()) {
                screen.Widget = widget;
                widget.Parent = null;
                if (true) {
                    screen.Activate( widget, argument );
                }
            }
        }
        internal static void RemoveWidgetInternal(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            using (screen.@lock.Enter()) {
                if (true) {
                    screen.Deactivate( widget, argument );
                }
                widget.Parent = null;
                screen.Widget = null!;
            }
            if (widget.DisposeWhenDetach) {
                widget.Dispose();
            }
        }

        // Activate
        internal static void Activate(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            widget.OnBeforeAttach( argument );
            widget.State = UIWidgetState.Attaching;
            widget.Screen = screen;
            {
                widget.OnAttach( argument );
                foreach (var child in widget.Children) {
                    screen.Activate( child, argument );
                }
            }
            widget.State = UIWidgetState.Attached;
            widget.OnAfterAttach( argument );
        }
        internal static void Deactivate(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            widget.OnBeforeDetach( argument );
            widget.State = UIWidgetState.Detaching;
            {
                foreach (var child in widget.Children.Reverse()) {
                    screen.Deactivate( child, argument );
                }
                widget.OnDetach( argument );
            }
            widget.Screen = null;
            widget.State = UIWidgetState.Detached;
            widget.OnAfterDetach( argument );
        }

    }
}

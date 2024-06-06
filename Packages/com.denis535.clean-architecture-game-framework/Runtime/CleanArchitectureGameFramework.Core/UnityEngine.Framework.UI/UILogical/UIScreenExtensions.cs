#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class UIScreenExtensions {

        // AddWidgetInternal
        internal static void AddWidgetInternal(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            using (screen.@lock.Enter()) {
                screen.Widget = widget;
                widget.Parent = null;
                if (true) {
                    widget.Activate( screen, argument );
                }
            }
        }
        internal static void RemoveWidgetInternal(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            using (screen.@lock.Enter()) {
                if (true) {
                    widget.Deactivate( screen, argument );
                }
                widget.Parent = null;
                screen.Widget = null!;
            }
        }

    }
}

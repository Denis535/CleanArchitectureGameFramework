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
                widget.Activate( screen, argument );
            }
        }
        internal static void RemoveWidgetInternal(this UIScreenBase screen, UIWidgetBase widget, object? argument) {
            using (screen.@lock.Enter()) {
                widget.Deactivate( screen, argument );
                widget.Parent = null;
                screen.Widget = null!;
            }
        }

    }
}

#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    public static class UIWidgetExtensions {

        // GetView
        public static UIViewBase? __GetView__(this UIWidgetBase widget) {
            // try not to use this method
            return widget.View;
        }
        public static T __GetView__<T>(this UIWidgetBase<T> widget) where T : notnull, UIViewBase {
            // try not to use this method
            return widget.View;
        }

        // GetEventCancellationToken
        public static CancellationToken GetEventCancellationToken_OnBeforeDetach(this UIWidgetBase widget) {
            var cts = new CancellationTokenSource();
            widget.OnBeforeDetachEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                widget.OnBeforeDetachEvent -= OnEvent;
            }
            return cts.Token;
        }
        public static CancellationToken GetEventCancellationToken_OnAfterDetach(this UIWidgetBase widget) {
            var cts = new CancellationTokenSource();
            widget.OnAfterDetachEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                widget.OnAfterDetachEvent -= OnEvent;
            }
            return cts.Token;
        }
        public static CancellationToken GetEventCancellationToken_OnBeforeDeactivate(this UIWidgetBase widget) {
            var cts = new CancellationTokenSource();
            widget.OnBeforeDeactivateEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                widget.OnBeforeDeactivateEvent -= OnEvent;
            }
            return cts.Token;
        }
        public static CancellationToken GetEventCancellationToken_OnAfterDeactivate(this UIWidgetBase widget) {
            var cts = new CancellationTokenSource();
            widget.OnAfterDeactivateEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                widget.OnAfterDeactivateEvent -= OnEvent;
            }
            return cts.Token;
        }

    }
}

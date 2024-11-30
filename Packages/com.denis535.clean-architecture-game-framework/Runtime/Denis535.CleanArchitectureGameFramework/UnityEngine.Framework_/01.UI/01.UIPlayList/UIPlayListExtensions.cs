#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    public static class UIPlayListExtensions {

        // GetEventCancellationToken
        public static CancellationToken GetEventCancellationToken_OnBeforeDetach(this UIPlayListBase playList) {
            var cts = new CancellationTokenSource();
            playList.OnBeforeDetachEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                playList.OnBeforeDetachEvent -= OnEvent;
            }
            return cts.Token;
        }
        public static CancellationToken GetEventCancellationToken_OnAfterDetach(this UIPlayListBase playList) {
            var cts = new CancellationTokenSource();
            playList.OnAfterDetachEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                playList.OnAfterDetachEvent -= OnEvent;
            }
            return cts.Token;
        }
        public static CancellationToken GetEventCancellationToken_OnBeforeDeactivate(this UIPlayListBase playList) {
            var cts = new CancellationTokenSource();
            playList.OnBeforeDeactivateEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                playList.OnBeforeDeactivateEvent -= OnEvent;
            }
            return cts.Token;
        }
        public static CancellationToken GetEventCancellationToken_OnAfterDeactivate(this UIPlayListBase playList) {
            var cts = new CancellationTokenSource();
            playList.OnAfterDeactivateEvent += OnEvent;
            void OnEvent(object? argument) {
                cts.Cancel();
                playList.OnAfterDeactivateEvent -= OnEvent;
            }
            return cts.Token;
        }

    }
}

#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ViewAttachEvent : EventBase<ViewAttachEvent> {

        public ViewAttachEvent() {
            tricklesDown = true;
            bubbles = true;
        }

        protected override void Init() {
            base.Init();
        }

        public static void Dispatch(VisualElement target) {
            using (var evt = GetPooled()) {
                evt.target = target;
                target.SendEvent( evt );
            }
        }

    }
}

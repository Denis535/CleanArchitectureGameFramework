#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Framework.UI;

    public class WidgetAttachEvent : EventBase<WidgetAttachEvent> {

        public UIWidgetBase? Widget { get; private set; }

        public WidgetAttachEvent() {
            tricklesDown = true;
            bubbles = true;
        }

        protected override void Init() {
            base.Init();
            tricklesDown = true;
            bubbles = true;
        }
        protected override void PreDispatch(IPanel panel) {
            base.PreDispatch( panel );
        }
        protected override void PostDispatch(IPanel panel) {
            base.PostDispatch( panel );
            Widget = null;
        }

        public static void Dispatch(VisualElement target, UIWidgetBase widget) {
            using (var evt = GetPooled()) {
                evt.Widget = widget;
                evt.target = target;
                target.SendEvent( evt );
            }
        }

    }
    public class WidgetDetachEvent : EventBase<WidgetDetachEvent> {

        public UIWidgetBase? Widget { get; private set; }

        public WidgetDetachEvent() {
            tricklesDown = true;
            bubbles = true;
        }

        protected override void Init() {
            base.Init();
            tricklesDown = true;
            bubbles = true;
        }
        protected override void PreDispatch(IPanel panel) {
            base.PreDispatch( panel );
        }
        protected override void PostDispatch(IPanel panel) {
            base.PostDispatch( panel );
            Widget = null;
        }

        public static void Dispatch(VisualElement target, UIWidgetBase widget) {
            using (var evt = GetPooled()) {
                evt.Widget = widget;
                evt.target = target;
                target.SendEvent( evt );
            }
        }

    }
}

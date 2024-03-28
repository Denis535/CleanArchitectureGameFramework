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
            Assert.Operation.Message( $"Target {target} must be attached" ).Valid( target.IsAttached() );
            using (var evt = GetPooled()) {
                evt.target = target;
                evt.Widget = widget;
                target.SendEventImmediate( evt );
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
            Assert.Operation.Message( $"Target {target} must be attached" ).Valid( target.IsAttached() );
            using (var evt = GetPooled()) {
                evt.target = target;
                evt.Widget = widget;
                target.SendEventImmediate( evt );
            }
        }

    }
}

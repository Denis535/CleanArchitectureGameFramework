#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RootWidget : UIWidgetBase<RootWidgetView> {

        // View
        public override RootWidgetView View { get; }

        // Constructor
        public RootWidget() {
            View = new RootWidgetView();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach() {
        }
        public override void OnDetach() {
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase widget) {
        }
        public override void OnAfterDescendantAttach(UIWidgetBase widget) {
            // show widget
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase widget) {
            // hide widget
        }
        public override void OnAfterDescendantDetach(UIWidgetBase widget) {
        }

    }
}

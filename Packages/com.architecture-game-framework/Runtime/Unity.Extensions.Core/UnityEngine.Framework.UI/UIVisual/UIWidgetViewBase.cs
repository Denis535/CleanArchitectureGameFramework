#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIWidgetViewBase : UIViewBase {

        // Widget
        internal UIWidgetBase Widget { get; }

        // Constructor
        public UIWidgetViewBase(UIWidgetBase widget) {
            Widget = widget;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UISubViewBase : UIViewBase {

        // Widget
        internal UIWidgetBase Widget { get; }

        // Constructor
        public UISubViewBase(UIWidgetBase widget) {
            Widget = widget;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

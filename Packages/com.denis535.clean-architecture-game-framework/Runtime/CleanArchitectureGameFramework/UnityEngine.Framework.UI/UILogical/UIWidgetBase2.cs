#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIWidgetBase2 : UIWidgetBase {

        // Container
        protected abstract IDependencyContainer Container { get; }

        // Constructor
        public UIWidgetBase2() {
        }

    }
}

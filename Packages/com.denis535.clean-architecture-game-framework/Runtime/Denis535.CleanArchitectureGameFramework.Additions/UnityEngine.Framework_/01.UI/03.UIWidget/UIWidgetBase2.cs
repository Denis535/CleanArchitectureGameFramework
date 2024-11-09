#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIWidgetBase2 : UIWidgetBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public UIWidgetBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class UIWidgetBase2<TView> : UIWidgetBase<TView> where TView : notnull, UIViewBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public UIWidgetBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

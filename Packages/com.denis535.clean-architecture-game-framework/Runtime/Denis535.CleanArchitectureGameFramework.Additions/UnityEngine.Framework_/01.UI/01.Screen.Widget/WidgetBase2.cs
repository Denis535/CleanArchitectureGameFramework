#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class WidgetBase2 : WidgetBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public WidgetBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class WidgetBase2<TView> : WidgetBase<TView> where TView : notnull, ViewBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public WidgetBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

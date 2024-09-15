#nullable enable
namespace UnityEngine.Framework.UI {
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

        // OnActivate
        protected override void OnBeforeActivate(object? argument) {
        }
        protected override void OnAfterActivate(object? argument) {
        }
        protected override void OnBeforeDeactivate(object? argument) {
        }
        protected override void OnAfterDeactivate(object? argument) {
            Dispose();
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

        // OnActivate
        protected override void OnBeforeActivate(object? argument) {
        }
        protected override void OnAfterActivate(object? argument) {
        }
        protected override void OnBeforeDeactivate(object? argument) {
        }
        protected override void OnAfterDeactivate(object? argument) {
            Dispose();
        }

    }
}

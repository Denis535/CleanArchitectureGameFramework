#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIThemeBase2 : UIThemeBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public UIThemeBase2(IDependencyContainer container, AudioSource audioSource) : base( audioSource ) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

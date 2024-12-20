#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ThemeBase2 : ThemeBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public ThemeBase2(IDependencyContainer container, AudioSource audioSource) : base( audioSource ) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

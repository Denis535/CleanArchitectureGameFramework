#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class ScreenBase2 : ScreenBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public ScreenBase2(IDependencyContainer container, UIDocument document, AudioSource audioSource) : base( document, audioSource ) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

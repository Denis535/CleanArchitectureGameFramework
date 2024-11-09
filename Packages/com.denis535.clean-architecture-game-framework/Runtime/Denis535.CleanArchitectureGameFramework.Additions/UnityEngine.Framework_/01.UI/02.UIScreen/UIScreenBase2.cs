#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase2 : UIScreenBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public UIScreenBase2(IDependencyContainer container, UIDocument document, AudioSource audioSource) : base( document, audioSource ) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

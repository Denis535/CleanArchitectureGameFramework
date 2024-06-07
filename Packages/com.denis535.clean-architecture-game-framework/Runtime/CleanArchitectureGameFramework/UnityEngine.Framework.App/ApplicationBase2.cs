#nullable enable
namespace UnityEngine.Framework.App {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ApplicationBase2 : ApplicationBase {

        // Container
        protected IDependencyContainer Container { get; }

        // Constructor
        public ApplicationBase2(IDependencyContainer container) {
            Container = container;
        }

    }
}

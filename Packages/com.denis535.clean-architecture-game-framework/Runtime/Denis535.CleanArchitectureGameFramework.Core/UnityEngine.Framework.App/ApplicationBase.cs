#nullable enable
namespace UnityEngine.Framework.App {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ApplicationBase : Disposable {

        // Constructor
        public ApplicationBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

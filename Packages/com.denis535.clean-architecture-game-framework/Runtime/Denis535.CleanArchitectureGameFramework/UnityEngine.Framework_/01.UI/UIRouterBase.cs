#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIRouterBase : DisposableBase {

        // Constructor
        public UIRouterBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Router {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.Dispose();
        }

    }
}

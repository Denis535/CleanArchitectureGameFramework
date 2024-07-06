#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase : Disposable {

        // Constructor
        public PlayerBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

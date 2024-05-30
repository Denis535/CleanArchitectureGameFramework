#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : Disposable {

        // Constructor
        public EntityBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

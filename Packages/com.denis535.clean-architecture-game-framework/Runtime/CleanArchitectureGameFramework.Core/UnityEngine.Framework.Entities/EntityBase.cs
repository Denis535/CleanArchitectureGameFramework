#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : MonoBehaviour {

        // Awake
        protected abstract void Awake();
        protected abstract void OnDestroy();

    }
    public abstract class EntityBodyBase : Disposable {

        // Constructor
        public EntityBodyBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class EntityViewBase : Disposable {

        // Constructor
        public EntityViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

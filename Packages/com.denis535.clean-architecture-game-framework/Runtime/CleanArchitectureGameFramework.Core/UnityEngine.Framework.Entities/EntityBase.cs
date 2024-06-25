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

        protected GameObject GameObject { get; }
        protected Transform Transform => GameObject.transform;

        // Constructor
        public EntityBodyBase(GameObject gameObject) {
            GameObject = gameObject;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class EntityViewBase : Disposable {

        protected GameObject GameObject { get; }
        protected Transform Transform => GameObject.transform;

        // Constructor
        public EntityViewBase(GameObject gameObject) {
            GameObject = gameObject;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

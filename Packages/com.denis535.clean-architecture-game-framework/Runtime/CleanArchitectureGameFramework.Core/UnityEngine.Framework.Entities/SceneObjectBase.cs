#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class SceneObjectBase : MonoBehaviour {

        // Entity
        public EntityBase Entity { get; protected set; } = default!;

        // Awake
        protected virtual void Awake() {
        }
        protected virtual void OnDestroy() {
        }

    }
    public abstract class SceneObjectBase<TEntity> : SceneObjectBase where TEntity : notnull, EntityBase {

        // Entity
        public new TEntity Entity {
            get => (TEntity) base.Entity;
            protected set => base.Entity = value;
        }

        // Awake
        protected override void Awake() {
            base.Awake();
        }
        protected override void OnDestroy() {
            base.OnDestroy();
        }

    }
}

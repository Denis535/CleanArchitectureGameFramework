#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Entity )]
    public abstract class EntityBase : MonoBehaviour {

        // Awake
        public virtual void Awake() {
        }
        public virtual void OnDestroy() {
        }

    }
    public abstract class EntityBase<TView> : EntityBase where TView : EntityViewBase {

        public TView View { get; protected set; } = default!;

        // Awake
        public override void Awake() {
            View = gameObject.RequireComponent<TView>();
        }
        public override void OnDestroy() {
        }

    }
    public abstract class EntityBase<TBody, TView> : EntityBase where TBody : EntityBodyBase where TView : EntityViewBase {

        public TBody Body { get; protected set; } = default!;
        public TView View { get; protected set; } = default!;

        // Awake
        public override void Awake() {
            Body = gameObject.RequireComponent<TBody>();
            View = gameObject.RequireComponent<TView>();
        }
        public override void OnDestroy() {
        }

    }
}

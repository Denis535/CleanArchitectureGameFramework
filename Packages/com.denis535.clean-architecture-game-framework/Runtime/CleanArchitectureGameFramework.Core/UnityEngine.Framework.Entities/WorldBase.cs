#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.World )]
    public abstract class WorldBase : MonoBehaviour {

        // Awake
        public virtual void Awake() {
        }
        public virtual void OnDestroy() {
        }

    }
    public abstract class WorldBase<TView> : WorldBase where TView : WorldViewBase {

        public TView View { get; protected set; } = default!;

        // Awake
        public override void Awake() {
            View = gameObject.RequireComponent<TView>();
        }
        public override void OnDestroy() {
        }

    }
}

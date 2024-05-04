#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Level )]
    public abstract class LevelBase : MonoBehaviour {

        // Awake
        public virtual void Awake() {
        }
        public virtual void OnDestroy() {
        }

    }
    public abstract class LevelBase<TView> : LevelBase where TView : LevelViewBase {

        public TView View { get; protected set; } = default!;

        // Awake
        public override void Awake() {
            View = gameObject.RequireComponent<TView>();
        }
        public override void OnDestroy() {
        }

    }
}

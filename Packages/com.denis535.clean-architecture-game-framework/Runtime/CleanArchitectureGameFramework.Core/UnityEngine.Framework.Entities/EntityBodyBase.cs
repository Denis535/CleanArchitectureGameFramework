#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Entity_Body )]
    public abstract class EntityBodyBase : MonoBehaviour {

        // Awake
        public virtual void Awake() {
        }
        public virtual void OnDestroy() {
        }

    }
}

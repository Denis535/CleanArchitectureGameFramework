#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Level_View )]
    public abstract class LevelViewBase : MonoBehaviour {

        // Awake
        public virtual void Awake() {
        }
        public virtual void OnDestroy() {
        }

    }
}

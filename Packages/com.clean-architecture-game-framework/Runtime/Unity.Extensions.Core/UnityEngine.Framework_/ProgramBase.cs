#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Program )]
    public abstract class ProgramBase : MonoBehaviour {

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
        }

    }
}

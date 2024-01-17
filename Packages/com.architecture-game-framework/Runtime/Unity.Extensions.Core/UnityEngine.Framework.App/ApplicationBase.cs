#nullable enable
namespace UnityEngine.Framework.App {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Application )]
    public abstract class ApplicationBase : MonoBehaviour {

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
        }

    }
}

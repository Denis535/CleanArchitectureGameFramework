#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.UIRouter )]
    public abstract class UIRouterBase : MonoBehaviour {

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
        }

    }
}

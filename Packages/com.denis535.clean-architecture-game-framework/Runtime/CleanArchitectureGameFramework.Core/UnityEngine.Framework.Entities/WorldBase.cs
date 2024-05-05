#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.World )]
    public abstract class WorldBase : MonoBehaviour {

        // Awake
        public abstract void Awake();
        public abstract void OnDestroy();

    }
    public abstract class WorldBase<TView> : WorldBase where TView : notnull, WorldViewBase {

        // View
        protected TView View { get; set; } = default!;

    }
}

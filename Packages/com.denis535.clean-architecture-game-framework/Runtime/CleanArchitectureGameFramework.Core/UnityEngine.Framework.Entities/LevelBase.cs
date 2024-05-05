#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Level )]
    public abstract class LevelBase : MonoBehaviour {

        // Awake
        public abstract void Awake();
        public abstract void OnDestroy();

    }
    public abstract class LevelBase<TView> : LevelBase where TView : notnull, LevelViewBase {

        protected TView View { get; set; } = default!;

    }
}

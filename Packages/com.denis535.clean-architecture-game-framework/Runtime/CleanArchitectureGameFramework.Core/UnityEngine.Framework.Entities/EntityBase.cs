#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.Entity )]
    public abstract class EntityBase : MonoBehaviour {

        // Awake
        public abstract void Awake();
        public abstract void OnDestroy();

    }
    public abstract class EntityBase<TView> : EntityBase where TView : notnull, EntityViewBase {

        // View
        protected abstract TView View { get; set; }

    }
    public abstract class EntityBase<TBody, TView> : EntityBase where TBody : notnull, EntityBodyBase where TView : notnull, EntityViewBase {

        // Body
        protected abstract TBody Body { get; set; }
        // View
        protected abstract TView View { get; set; }

    }
}

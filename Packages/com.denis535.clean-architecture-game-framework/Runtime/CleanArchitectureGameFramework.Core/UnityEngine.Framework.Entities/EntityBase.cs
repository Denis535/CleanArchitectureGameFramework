#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : Disposable {

        // GameObject
        protected GameObject GameObject { get; init; } = default!;
        // Transform
        protected Transform Transform => GameObject.transform;

        // Constructor
        public EntityBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class EntityBase<TBody, TView> : EntityBase where TBody : notnull, BodyBase where TView : notnull, ViewBase {

        // Body
        protected TBody Body { get; init; } = default!;
        // View
        protected TView View { get; init; } = default!;

        // Constructor
        public EntityBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    // BodyBase
    public abstract class BodyBase : Disposable {

        // GameObject
        protected GameObject GameObject { get; init; } = default!;
        // Transform
        protected Transform Transform => GameObject.transform;

        // Constructor
        public BodyBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    // ViewBase
    public abstract class ViewBase : Disposable {

        // GameObject
        protected GameObject GameObject { get; init; } = default!;
        // Transform
        protected Transform Transform => GameObject.transform;

        // Constructor
        public ViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

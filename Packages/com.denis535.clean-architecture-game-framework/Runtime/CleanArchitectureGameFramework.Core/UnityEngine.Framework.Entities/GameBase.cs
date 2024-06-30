#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GameBase : Disposable {

        // Constructor
        public GameBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // RegisterEntity
        protected internal abstract void RegisterEntity(EntityBase entity);
        protected internal abstract void UnregisterEntity(EntityBase entity);

    }
}

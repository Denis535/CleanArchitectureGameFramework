#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : Disposable {

        // System
        protected IEntityRegistry Registry { get; }

        // Constructor
        public EntityBase(IEntityRegistry registry) {
            Registry = registry;
            Registry.RegisterEntity( this );
        }
        public override void Dispose() {
            base.Dispose();
            Registry.UnregisterEntity( this );
        }

    }
}

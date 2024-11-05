#nullable enable
namespace UnityEngine.Framework.Game_.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : DisposableBase {

        // System
        protected IEntityRegistry Registry { get; }

        // Constructor
        public EntityBase(IEntityRegistry registry) {
            Registry = registry;
            Registry.RegisterEntity( this );
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Entity {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.Dispose();
            Registry.UnregisterEntity( this );
        }

    }
}

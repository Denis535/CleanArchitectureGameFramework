#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GameBase2 : GameBase, IEntityRegistry {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public GameBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // IEntityRegistry
        void IEntityRegistry.RegisterEntity(EntityBase entity) {
            RegisterEntity( entity );
        }
        void IEntityRegistry.UnregisterEntity(EntityBase entity) {
            UnregisterEntity( entity );
        }
        protected abstract void RegisterEntity(EntityBase entity);
        protected abstract void UnregisterEntity(EntityBase entity);

    }
}

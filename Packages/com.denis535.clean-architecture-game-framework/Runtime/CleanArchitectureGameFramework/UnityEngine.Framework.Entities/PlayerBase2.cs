#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase2 : PlayerBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public PlayerBase2(IDependencyContainer container, GameBase game) : base( game ) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

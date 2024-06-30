#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase : Disposable {

        // Game
        protected GameBase Game { get; }

        // Constructor
        public PlayerBase(GameBase game) {
            Game = game;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

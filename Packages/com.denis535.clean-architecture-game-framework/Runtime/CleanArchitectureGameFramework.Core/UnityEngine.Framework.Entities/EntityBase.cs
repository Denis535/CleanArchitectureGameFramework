#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : Disposable {

        // Game
        protected GameBase Game { get; }

        // Constructor
        public EntityBase(GameBase game) {
            Game = game;
            Game.RegisterEntity( this );
        }
        public override void Dispose() {
            Game.UnregisterEntity( this );
            base.Dispose();
        }

    }
    public abstract class EntityBase<TBody, TView> : EntityBase where TBody : notnull, BodyBase where TView : notnull, ViewBase {

        // Body
        protected TBody Body { get; }
        // View
        protected TView View { get; }

        // Constructor
        public EntityBase(GameBase game, TBody body, TView view) : base( game ) {
            Body = body;
            View = view;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    // BodyBase
    public abstract class BodyBase : Disposable {

        // Constructor
        public BodyBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    // ViewBase
    public abstract class ViewBase : Disposable {

        // Constructor
        public ViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

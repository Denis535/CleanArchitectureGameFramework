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
        public PlayerBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class PlayerBase2<TKind, TState> : PlayerBase2 where TKind : Enum where TState : Enum {

        private TState state = default!;

        // Name
        public abstract string Name { get; }
        // Kind
        public abstract TKind Kind { get; }
        // State
        public virtual TState State {
            get => state;
            set {
                Assert.Operation.Message( $"Transition from {state} to {value} is invalid" ).Valid( !EqualityComparer<TState>.Default.Equals( value, state ) );
                state = value;
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<TState>? OnStateChangeEvent;

        // Constructor
        public PlayerBase2(IDependencyContainer container) : base( container ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

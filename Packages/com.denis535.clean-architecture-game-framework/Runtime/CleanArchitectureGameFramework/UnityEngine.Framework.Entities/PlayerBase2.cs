#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase2<TState> : PlayerBase where TState : Enum {

        private TState state = default!;

        // System
        protected IDependencyContainer Container { get; }
        // State
        public TState State {
            get => state;
            protected internal set {
                Assert.Operation.Message( $"Transition from {state} to {value} is invalid" ).Valid( !EqualityComparer<TState>.Default.Equals( value, state ) );
                state = value;
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<TState>? OnStateChangeEvent;

        // Constructor
        public PlayerBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class PlayerBase2<TKind, TState> : PlayerBase2<TState> where TKind : Enum where TState : Enum {

        // Name
        public string Name { get; }
        // Kind
        public TKind Kind { get; }

        // Constructor
        public PlayerBase2(IDependencyContainer container, string name, TKind kind) : base( container ) {
            Name = name;
            Kind = kind;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

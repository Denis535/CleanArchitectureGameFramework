#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase2<TKind, TState> : PlayerBase
        where TKind : Enum
        where TState : Enum {

        private TState state = default!;

        // Name
        public string Name { get; }
        // Kind
        public TKind Kind { get; }
        // State
        public TState State {
            get => state;
            set {
                var prev = state;
                state = value;
                OnStateChange( state );
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<TState>? OnStateChangeEvent;
        // Container
        protected abstract IDependencyContainer Container { get; }

        // Constructor
        public PlayerBase2(string name, TKind kind) {
            Name = name;
            Kind = kind;
        }

        // OnStateChange
        protected abstract void OnStateChange(TState state);

    }
}

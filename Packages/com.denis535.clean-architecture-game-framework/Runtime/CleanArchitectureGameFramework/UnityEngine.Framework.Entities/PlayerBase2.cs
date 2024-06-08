#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase2<TKind, TInput> : PlayerBase where TKind : Enum where TInput : IDisposable, new() {

        private PlayerState state;

        // Container
        protected IDependencyContainer Container { get; }
        // Name
        public string Name { get; }
        // Kind
        public TKind Kind { get; }
        // State
        public PlayerState State {
            get => state;
            set {
                var prev = state;
                state = GetState( value, prev );
                OnStateChange( state );
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<PlayerState>? OnStateChangeEvent;
        // Input
        protected TInput Input { get; }

        // Constructor
        public PlayerBase2(IDependencyContainer container, string name, TKind kind) {
            Container = container;
            Name = name;
            Kind = kind;
            Input = new TInput();
        }
        public override void Dispose() {
            Input.Dispose();
            base.Dispose();
        }

        // Update
        public abstract void Update();

        // OnStateChange
        protected abstract void OnStateChange(PlayerState state);

        // Helpers
        private static PlayerState GetState(PlayerState state, PlayerState prev) {
            switch (state) {
                case PlayerState.Winner:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is PlayerState.Playing );
                    return state;
                case PlayerState.Looser:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is PlayerState.Playing );
                    return state;
                default:
                    throw Exceptions.Internal.NotSupported( $"Transition from {prev} to {state} is not supported" );
            }
        }

    }
    public enum PlayerState {
        Playing,
        Winner,
        Looser
    }
}

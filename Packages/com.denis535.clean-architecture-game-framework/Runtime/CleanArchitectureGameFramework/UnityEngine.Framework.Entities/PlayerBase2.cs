#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase2 : PlayerBase {

        private PlayerState state;

        // Container
        protected IDependencyContainer Container { get; }
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

        // Constructor
        public PlayerBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnStateChange
        protected virtual void OnStateChange(PlayerState state) {
        }

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

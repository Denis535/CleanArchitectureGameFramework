#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GameBase2 : GameBase {

        private GameState state;
        private bool isPaused;

        // Container
        protected IDependencyContainer Container { get; }
        // State
        public GameState State {
            get => state;
            protected set {
                var prev = state;
                state = GetState( value, prev );
                OnStateChange( state );
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<GameState>? OnStateChangeEvent;
        // IsPaused
        public bool IsPaused {
            get => isPaused;
            set {
                if (value != isPaused) {
                    isPaused = value;
                    OnPauseChange( isPaused );
                    OnPauseChangeEvent?.Invoke( isPaused );
                }
            }
        }
        public event Action<bool>? OnPauseChangeEvent;

        // Constructor
        public GameBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Update
        public virtual void FixedUpdate() {
        }
        public virtual void Update() {
        }
        public virtual void LateUpdate() {
        }

        // OnStateChange
        protected virtual void OnStateChange(GameState state) {
        }

        // OnPauseChange
        protected virtual void OnPauseChange(bool isPaused) {
        }

        // Helpers
        private static GameState GetState(GameState state, GameState prev) {
            switch (state) {
                case GameState.Completed:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is GameState.Playing );
                    return state;
                default:
                    throw Exceptions.Internal.NotSupported( $"Transition from {prev} to {state} is not supported" );
            }
        }

    }
    public enum GameState {
        Playing,
        Completed
    }
}

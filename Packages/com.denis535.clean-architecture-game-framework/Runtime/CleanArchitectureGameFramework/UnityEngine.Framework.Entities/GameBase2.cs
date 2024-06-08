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
                OnStateChangeEvent?.Invoke( this, state, prev );
            }
        }
        public event Action<GameBase2, GameState, GameState>? OnStateChangeEvent;
        // IsPaused
        public virtual bool IsPaused {
            get => isPaused;
            protected set {
                if (value) {
                    if (!IsPaused) {
                        isPaused = true;
                        OnPauseEvent?.Invoke( this, true );
                    }
                } else {
                    if (IsPaused) {
                        isPaused = false;
                        OnPauseEvent?.Invoke( this, false );
                    }
                }
            }
        }
        public event Action<GameBase2, bool>? OnPauseEvent;

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

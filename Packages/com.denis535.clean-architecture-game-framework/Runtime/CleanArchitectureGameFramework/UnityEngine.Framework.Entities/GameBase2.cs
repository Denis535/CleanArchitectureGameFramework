#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GameBase2<TState> : GameBase where TState : Enum {

        private TState state = default!;
        private bool isPaused;

        // System
        protected IDependencyContainer Container { get; }
        // State
        public TState State {
            get => state;
            protected set {
                Assert.Operation.Message( $"Transition from {state} to {value} is invalid" ).Valid( !EqualityComparer<TState>.Default.Equals( value, state ) );
                state = value;
                OnStateChange( state );
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<TState>? OnStateChangeEvent;
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
        public abstract void FixedUpdate();
        public abstract void Update();
        public abstract void LateUpdate();

        // OnStateChange
        protected abstract void OnStateChange(TState state);

        // OnPauseChange
        protected abstract void OnPauseChange(bool isPaused);

        // Helpers
        protected static void SetState<T>(PlayerBase2<T> player, T state) where T : Enum {
            player.State = state;
        }

    }
    public abstract class GameBase2<TMode, TLevel, TState> : GameBase2<TState> where TState : Enum {

        // Name
        public string Name { get; }
        // Mode
        public TMode Mode { get; }
        // Level
        public TLevel Level { get; }

        // Constructor
        public GameBase2(IDependencyContainer container, string name, TMode mode, TLevel level) : base( container ) {
            Name = name;
            Mode = mode;
            Level = level;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

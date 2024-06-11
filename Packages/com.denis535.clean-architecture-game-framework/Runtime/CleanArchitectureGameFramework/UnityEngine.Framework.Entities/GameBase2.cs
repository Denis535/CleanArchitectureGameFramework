#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GameBase2<TMode, TLevel, TState> : GameBase
        where TMode : Enum
        where TLevel : Enum
        where TState : Enum {

        private TState state = default!;
        private bool isPaused;

        // Name
        public string Name { get; }
        // Mode
        public TMode Mode { get; }
        // Level
        public TLevel Level { get; }
        // State
        public TState State {
            get => state;
            protected set {
                var prev = state;
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
        // Container
        protected abstract IDependencyContainer Container { get; }

        // Constructor
        public GameBase2(string name, TMode mode, TLevel level) {
            Name = name;
            Mode = mode;
            Level = level;
        }

        // Update
        public abstract void FixedUpdate();
        public abstract void Update();
        public abstract void LateUpdate();

        // OnStateChange
        protected abstract void OnStateChange(TState state);

        // OnPauseChange
        protected abstract void OnPauseChange(bool isPaused);

    }
}

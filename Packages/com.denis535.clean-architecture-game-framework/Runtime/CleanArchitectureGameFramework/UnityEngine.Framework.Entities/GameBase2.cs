#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class GameBase2 : GameBase {

        private bool isPaused;

        // System
        protected IDependencyContainer Container { get; }
        // IsPaused
        public bool IsPaused {
            get => isPaused;
            set {
                if (value != isPaused) {
                    isPaused = value;
                    OnPauseEvent?.Invoke( isPaused );
                }
            }
        }
        public event Action<bool>? OnPauseEvent;

        // Constructor
        public GameBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

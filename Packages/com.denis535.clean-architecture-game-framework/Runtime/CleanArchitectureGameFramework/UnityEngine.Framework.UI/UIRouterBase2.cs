#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIRouterBase2<TState> : UIRouterBase
        where TState : Enum {

        private TState state = default!;

        // State
        public TState State {
            get => state;
            protected set {
                var prev = state;
                state = value;
                OnStateChangeEvent?.Invoke( state );
            }
        }
        public event Action<TState>? OnStateChangeEvent;
        // Container
        protected abstract IDependencyContainer Container { get; }

        // Constructor
        public UIRouterBase2() {
        }

    }
}

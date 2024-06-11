#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIRouterBase2<TState> : UIRouterBase
        where TState : Enum {

        private TState state = default!;

        // System
        protected IDependencyContainer Container { get; }
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

        // Constructor
        public UIRouterBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIRouterBase2 : UIRouterBase {

        // System
        protected IDependencyContainer Container { get; }

        // Constructor
        public UIRouterBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class UIRouterBase2<TState> : UIRouterBase2 where TState : Enum {

        private TState state = default!;

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

        // Constructor
        public UIRouterBase2(IDependencyContainer container) : base( container ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnStateChange
        protected abstract void OnStateChange(TState state);

        // Helpers
        protected static void SetState<T>(UIThemeBase2<T> theme, T state) where T : Enum {
            theme.State = state;
        }
        protected static void SetState<T>(UIScreenBase2<T> screen, T state) where T : Enum {
            screen.State = state;
        }

    }
}

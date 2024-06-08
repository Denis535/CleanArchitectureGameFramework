#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIRouterBase2 : UIRouterBase {

        private UIRouterState state;

        // Container
        protected IDependencyContainer Container { get; }
        // State
        public UIRouterState State {
            get => state;
            protected set {
                var prev = state;
                state = GetState( value, prev );
                OnStateChangeEvent?.Invoke( state, prev );
            }
        }
        public event Action<UIRouterState, UIRouterState>? OnStateChangeEvent;

        // Constructor
        public UIRouterBase2(IDependencyContainer container) {
            Container = container;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        private static UIRouterState GetState(UIRouterState state, UIRouterState prev) {
            switch (state) {
                case UIRouterState.MainSceneLoading:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is UIRouterState.None or UIRouterState.GameSceneLoaded );
                    return state;
                case UIRouterState.MainSceneLoaded:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is UIRouterState.MainSceneLoading );
                    return state;
                case UIRouterState.GameSceneLoading:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is UIRouterState.None or UIRouterState.MainSceneLoaded );
                    return state;
                case UIRouterState.GameSceneLoaded:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is UIRouterState.GameSceneLoading );
                    return state;
                case UIRouterState.Quitting:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is UIRouterState.MainSceneLoaded or UIRouterState.GameSceneLoaded );
                    return state;
                case UIRouterState.Quited:
                    Assert.Operation.Message( $"Transition from {prev} to {state} is invalid" ).Valid( prev is UIRouterState.Quitting );
                    return state;
                default:
                    throw Exceptions.Internal.NotSupported( $"Transition from {prev} to {state} is not supported" );
            }
        }

    }
    public enum UIRouterState {
        None,
        // MainSceneLoading
        MainSceneLoading,
        MainSceneLoaded,
        // GameSceneLoading
        GameSceneLoading,
        GameSceneLoaded,
        // Quitting
        Quitting,
        Quited,
    }
}

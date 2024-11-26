#nullable enable
namespace System.StateMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class StateBase2<TThis> : StateBase<TThis> where TThis : StateBase2<TThis> {

        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;

        // Constructor
        public StateBase2() {
        }

        // Activate
        private protected sealed override void Activate(object? argument) {
            Assert.Operation.Message( $"State {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            OnBeforeActivate( argument );
            Activity = Activity_.Activating;
            {
                OnActivate( argument );
            }
            Activity = Activity_.Active;
            OnAfterActivate( argument );
        }
        private protected sealed override void Deactivate(object? argument) {
            Assert.Operation.Message( $"State {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"State {this} must be active" ).Valid( Activity is Activity_.Active );
            OnBeforeDeactivate( argument );
            Activity = Activity_.Deactivating;
            {
                OnDeactivate( argument );
            }
            Activity = Activity_.Inactive;
            OnAfterDeactivate( argument );
        }

        // OnActivate
        protected virtual void OnBeforeActivate(object? argument) {
            OnBeforeActivateEvent?.Invoke( argument );
        }
        protected abstract void OnActivate(object? argument);
        protected virtual void OnAfterActivate(object? argument) {
            OnAfterActivateEvent?.Invoke( argument );
        }
        protected virtual void OnBeforeDeactivate(object? argument) {
            OnBeforeDeactivateEvent?.Invoke( argument );
        }
        protected abstract void OnDeactivate(object? argument);
        protected virtual void OnAfterDeactivate(object? argument) {
            OnAfterDeactivateEvent?.Invoke( argument );
        }

    }
}

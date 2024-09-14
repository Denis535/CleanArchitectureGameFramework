#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class StateBase {
        public enum State_ {
            Inactive,
            Activating,
            Active,
            Deactivating,
        }

        // Constructor
        internal StateBase() {
        }

    }
    public abstract class StateBase<T> : StateBase where T : StateBase<T> {

        // State
        public State_ State { get; private set; } = State_.Inactive;
        // Owner
        public IStateful<T>? Owner { get; private set; }
        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;

        // Constructor
        public StateBase() {
        }

        // Activate
        internal void Activate(IStateful<T> owner, object? argument) {
            Owner = owner;
            Activate( argument );
        }
        internal void Deactivate(IStateful<T> owner, object? argument) {
            Assert.Argument.Message( $"Argument 'owner' ({owner}) must be valid" ).Valid( owner == Owner );
            Deactivate( argument );
            Owner = null;
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( State is State_.Inactive );
            OnBeforeActivateEvent?.Invoke( argument );
            OnBeforeActivate( argument );
            {
                State = State_.Activating;
                OnActivate( argument );
                State = State_.Active;
            }
            OnAfterActivate( argument );
            OnAfterActivateEvent?.Invoke( argument );
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"State {this} must be active" ).Valid( State is State_.Active );
            OnBeforeDeactivateEvent?.Invoke( argument );
            OnBeforeDeactivate( argument );
            {
                State = State_.Deactivating;
                OnDeactivate( argument );
                State = State_.Inactive;
            }
            OnAfterDeactivate( argument );
            OnAfterDeactivateEvent?.Invoke( argument );
        }

        // OnActivate
        protected virtual void OnBeforeActivate(object? argument) {
        }
        protected abstract void OnActivate(object? argument);
        protected virtual void OnAfterActivate(object? argument) {
        }
        protected virtual void OnBeforeDeactivate(object? argument) {
        }
        protected abstract void OnDeactivate(object? argument);
        protected virtual void OnAfterDeactivate(object? argument) {
        }

    }
}

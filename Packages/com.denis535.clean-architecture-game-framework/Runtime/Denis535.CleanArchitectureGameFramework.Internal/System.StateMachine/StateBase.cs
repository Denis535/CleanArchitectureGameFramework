#nullable enable
namespace System.StateMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class StateBase<TThis> where TThis : StateBase<TThis> {
        public enum Activity_ {
            Inactive,
            Activating,
            Active,
            Deactivating,
        }

        // Owner
        private IStateful<TThis>? Owner { get; set; } = null;
        // Activity
        public Activity_ Activity { get; private set; } = Activity_.Inactive;

        // Stateful
        public IStateful<TThis>? Stateful => Owner;

        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;

        // Constructor
        public StateBase() {
        }

        // Attach
        internal void Attach(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must have no owner" ).Valid( Owner == null );
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            Owner = owner;
            //OnAttach( argument );
            Activate( argument );
        }
        internal void Detach(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must have {owner} owner" ).Valid( Owner == owner );
            Assert.Operation.Message( $"State {this} must be active" ).Valid( Activity is Activity_.Active );
            Deactivate( argument );
            //OnDetach( argument );
            Owner = null;
        }

        //// OnAttach
        //protected abstract void OnAttach(object? argument);
        //protected abstract void OnDetach(object? argument);

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"State {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            OnBeforeActivateInternal( argument );
            Activity = Activity_.Activating;
            {
                OnActivate( argument );
            }
            Activity = Activity_.Active;
            OnAfterActivateInternal( argument );
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"State {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"State {this} must be active" ).Valid( Activity is Activity_.Active );
            OnBeforeDeactivateInternal( argument );
            Activity = Activity_.Deactivating;
            {
                OnDeactivate( argument );
            }
            Activity = Activity_.Inactive;
            OnAfterDeactivateInternal( argument );
        }

        // OnActivate
        private void OnBeforeActivateInternal(object? argument) {
            OnBeforeActivateEvent?.Invoke( argument );
            OnBeforeActivate( argument );
        }
        private void OnAfterActivateInternal(object? argument) {
            OnAfterActivate( argument );
            OnAfterActivateEvent?.Invoke( argument );
        }
        private void OnBeforeDeactivateInternal(object? argument) {
            OnBeforeDeactivateEvent?.Invoke( argument );
            OnBeforeDeactivate( argument );
        }
        private void OnAfterDeactivateInternal(object? argument) {
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

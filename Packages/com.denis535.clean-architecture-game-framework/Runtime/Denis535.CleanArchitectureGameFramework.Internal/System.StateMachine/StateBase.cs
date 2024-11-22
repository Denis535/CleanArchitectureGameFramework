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

        // Activity
        public Activity_ Activity { get; private set; } = Activity_.Inactive;
        // Owner
        private IStateful<TThis>? Owner { get; set; }
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
        protected virtual void DisposeWhenDeactivate() {
            (this as IDisposable)?.Dispose();
        }

        // SetOwner
        internal void SetOwner(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            Assert.Operation.Message( $"State {this} must have no owner" ).Valid( Owner == null );
            Owner = owner;
            Activate( argument );
        }
        internal void RemoveOwner(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must be active" ).Valid( Activity is Activity_.Active );
            Assert.Operation.Message( $"State {this} must have {owner} owner" ).Valid( Owner == owner );
            Deactivate( argument );
            Owner = null;
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            BeforeActivate( argument );
            Activity = Activity_.Activating;
            {
                OnActivate( argument );
            }
            Activity = Activity_.Active;
            AfterActivate( argument );
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"State {this} must be active" ).Valid( Activity is Activity_.Active );
            BeforeDeactivate( argument );
            Activity = Activity_.Deactivating;
            {
                OnDeactivate( argument );
            }
            Activity = Activity_.Inactive;
            AfterDeactivate( argument );
            DisposeWhenDeactivate();
        }

        // Activate
        private void BeforeActivate(object? argument) {
            OnBeforeActivateEvent?.Invoke( argument );
            OnBeforeActivate( argument );
        }
        private void AfterActivate(object? argument) {
            OnAfterActivate( argument );
            OnAfterActivateEvent?.Invoke( argument );
        }
        private void BeforeDeactivate(object? argument) {
            OnBeforeDeactivateEvent?.Invoke( argument );
            OnBeforeDeactivate( argument );
        }
        private void AfterDeactivate(object? argument) {
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

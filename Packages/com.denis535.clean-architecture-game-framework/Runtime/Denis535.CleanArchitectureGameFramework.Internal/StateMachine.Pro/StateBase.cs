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

        // State
        public State_ State { get; private protected set; } = State_.Inactive;
        // Owner
        private protected IStateful? Owner { get; set; }
        // Stateful
        public IStateful? Stateful => Owner;

        // Constructor
        internal StateBase() {
        }

    }
    public abstract class StateBase<TThis> : StateBase where TThis : StateBase<TThis> {

        // Stateful
        public new IStateful<TThis>? Stateful => (IStateful<TThis>?) base.Stateful;
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

        // Activate
        internal void Activate(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( State is State_.Inactive );
            Assert.Operation.Message( $"State {this} must have no owner" ).Valid( Owner == null );
            Owner = owner;
            Activate( argument );
        }
        internal void Deactivate(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must be active" ).Valid( State is State_.Active );
            Assert.Operation.Message( $"State {this} must have {owner} owner" ).Valid( Owner == owner );
            Deactivate( argument );
            Owner = null;
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( State is State_.Inactive );
            {
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
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"State {this} must be active" ).Valid( State is State_.Active );
            {
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
            DisposeWhenDeactivate();
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

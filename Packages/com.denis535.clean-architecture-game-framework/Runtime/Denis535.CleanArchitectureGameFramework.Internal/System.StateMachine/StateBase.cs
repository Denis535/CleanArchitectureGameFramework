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
        private protected IStateful<TThis>? Owner { get; private set; } = null;
        // Activity
        public Activity_ Activity { get; private protected set; } = Activity_.Inactive;

        // Stateful
        public IStateful<TThis>? Stateful => Owner;

        // OnAttach
        public event Action<object?>? OnBeforeAttachEvent;
        public event Action<object?>? OnAfterAttachEvent;
        public event Action<object?>? OnBeforeDetachEvent;
        public event Action<object?>? OnAfterDetachEvent;

        // Constructor
        private protected StateBase() {
        }

        // Attach
        internal void Attach(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must have no owner" ).Valid( Owner == null );
            Assert.Operation.Message( $"State {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            {
                Owner = owner;
                OnBeforeAttach( argument );
                OnAttach( argument );
                OnAfterAttach( argument );
            }
            Activate( argument );
        }
        internal void Detach(IStateful<TThis> owner, object? argument) {
            Assert.Operation.Message( $"State {this} must have {owner} owner" ).Valid( Owner == owner );
            Assert.Operation.Message( $"State {this} must be active" ).Valid( Activity is Activity_.Active );
            Deactivate( argument );
            {
                OnBeforeDetach( argument );
                OnDetach( argument );
                OnAfterDetach( argument );
                Owner = null;
            }
        }

        // OnAttach
        protected virtual void OnBeforeAttach(object? argument) {
            OnBeforeAttachEvent?.Invoke( argument );
        }
        protected abstract void OnAttach(object? argument);
        protected virtual void OnAfterAttach(object? argument) {
            OnAfterAttachEvent?.Invoke( argument );
        }
        protected virtual void OnBeforeDetach(object? argument) {
            OnBeforeDetachEvent?.Invoke( argument );
        }
        protected abstract void OnDetach(object? argument);
        protected virtual void OnAfterDetach(object? argument) {
            OnAfterDetachEvent?.Invoke( argument );
        }

        // Activate
        private protected abstract void Activate(object? argument);
        private protected abstract void Deactivate(object? argument);

    }
}

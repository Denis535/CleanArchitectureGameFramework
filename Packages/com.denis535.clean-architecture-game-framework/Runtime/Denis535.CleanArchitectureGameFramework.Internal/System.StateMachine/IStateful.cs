#nullable enable
namespace System.StateMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IStateful<T> where T : StateBase<T> {

        // State
        protected T? State { get; set; }

        // SetState
        protected void SetState(T? state, object? argument, Action<T>? onRemoved);
        protected void AddState(T state, object? argument);
        protected void RemoveState(T state, object? argument, Action<T>? onRemoved);

        // Helpers
        protected static void SetState(IStateful<T> stateful, T? state, object? argument, Action<T>? onRemoved) {
            if (stateful.State != null) {
                stateful.RemoveState( stateful.State, argument, onRemoved );
            }
            if (state != null) {
                stateful.AddState( state, argument );
            }
        }
        protected static void AddState(IStateful<T> stateful, T state, object? argument) {
            Assert.Argument.Message( $"Argument 'state' must be non-null" ).NotNull( state != null );
            Assert.Operation.Message( $"Stateful {stateful} must have no state" ).Valid( stateful.State == null );
            stateful.State = state;
            stateful.State.Attach( stateful, argument );
        }
        protected static void RemoveState(IStateful<T> stateful, T state, object? argument, Action<T>? onRemoved) {
            Assert.Argument.Message( $"Argument 'state' must be non-null" ).NotNull( state != null );
            Assert.Operation.Message( $"Stateful {stateful} must have {state} state" ).Valid( stateful.State == state );
            stateful.State.Detach( stateful, argument );
            stateful.State = null;
            onRemoved?.Invoke( state );
        }

    }
}

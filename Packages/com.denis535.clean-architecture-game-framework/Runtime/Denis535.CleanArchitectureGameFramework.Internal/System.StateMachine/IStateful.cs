#nullable enable
namespace System.StateMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IStateful<T> where T : StateBase<T> {

        // State
        protected T? State { get; set; }

        // SetState
        protected internal void SetState(T? state, object? argument = null);

        // Helpers
        protected static void SetState(IStateful<T> stateful, T? state, object? argument) {
            if (stateful.State != null) {
                stateful.State.RemoveOwner( stateful, argument );
                stateful.State = null;
            }
            if (state != null) {
                stateful.State = state;
                stateful.State.SetOwner( stateful, argument );
            }
        }

    }
}

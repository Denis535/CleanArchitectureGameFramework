#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IStateful {
    }
    public interface IStateful<T> : IStateful where T : StateBase<T> {

        // State
        protected T? State { get; set; }

        // SetState
        protected internal void SetState(T? state, object? argument = null);

        // Helpers
        protected static void SetState(IStateful<T> stateful, T? state, object? argument) {
            if (state != null) {
                Assert.Argument.Message( $"Argument 'state' ({state}) must be valid" ).Valid( state != stateful.State );
            }
            if (stateful.State != null) {
                stateful.State.Deactivate( stateful, argument );
                stateful.State = null;
            }
            if (state != null) {
                stateful.State = state;
                stateful.State.Activate( stateful, argument );
            }
        }

    }
}

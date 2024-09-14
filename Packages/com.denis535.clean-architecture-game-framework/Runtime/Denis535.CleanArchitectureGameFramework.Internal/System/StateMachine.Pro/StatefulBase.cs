#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class StatefulBase<T> : IStateful<T> where T : StateBase<T> {

        // State
        T? IStateful<T>.State { get => State; set => State = value; }
        // State
        protected T? State { get; private set; }

        // Constructor
        public StatefulBase() {
        }

        // SetState
        void IStateful<T>.SetState(T? state, object? argument) => SetState( state, argument );
        // SetState
        protected virtual void SetState(T? state, object? argument = null) {
            IStateful<T>.SetState( this, state, argument );
        }

    }
}

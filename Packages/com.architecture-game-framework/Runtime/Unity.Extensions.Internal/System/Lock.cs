#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Lock {

        private volatile bool isLocked;
        public bool IsLocked { get => isLocked; internal set => isLocked = value; }

        public Lock() {
            IsLocked = false;
        }

        public IDisposable Enter() {
            Assert.Operation.Message( $"You are trying to enter locked scope" ).Valid( !IsLocked );
            return new LockScope( this );
        }

    }
    internal class LockScope : IDisposable {

        private readonly Lock @lock;

        public LockScope(Lock @lock) {
            this.@lock = @lock;
            this.@lock.IsLocked = true;
        }

        public void Dispose() {
            @lock.IsLocked = false;
        }

    }
}

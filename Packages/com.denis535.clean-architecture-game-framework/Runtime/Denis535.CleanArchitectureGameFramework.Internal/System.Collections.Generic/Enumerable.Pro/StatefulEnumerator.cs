#nullable enable
namespace System.Collections.Generic {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class StatefulEnumerator<T> : IEnumerator<T> {

        private IEnumerator<T> Source { get; }
        public bool IsStarted { get; private set; }
        public bool IsCompleted { get; private set; }
        public Option<T> Current { get; private set; }

        // Constructor
        public StatefulEnumerator(IEnumerator<T> source) {
            Source = source;
        }
        public void Dispose() {
            Source.Dispose();
        }

        // IEnumerator
        object? IEnumerator.Current => Current.Value;
        bool IEnumerator.MoveNext() => Take().HasValue;
        // IEnumerator<T>
        T IEnumerator<T>.Current => Current.Value;

        // Take
        public Option<T> Take() {
            if (Source.MoveNext()) {
                (IsStarted, IsCompleted) = (true, false);
                Current = Option.Create( Source.Current );
                return Current;
            }
            (IsStarted, IsCompleted) = (true, true);
            Current = default;
            return Current;
        }

        // Reset
        public void Reset() {
            Source.Reset();
            (IsStarted, IsCompleted) = (false, false);
            Current = default;
        }

    }
}

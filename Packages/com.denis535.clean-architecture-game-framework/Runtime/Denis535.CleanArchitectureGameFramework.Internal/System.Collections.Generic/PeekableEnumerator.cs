#nullable enable
namespace System.Collections.Generic {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class PeekableEnumerator<T> : IEnumerator<T> {

        private IEnumerator<T> Source { get; }
        public bool IsStarted { get; private set; }
        public bool IsFinished { get; private set; }
        public Option<T> Current { get; private set; }
        private Option<T> Next { get; set; }

        // Constructor
        public PeekableEnumerator(IEnumerator<T> source) {
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
            if (Next.HasValue) {
                (IsStarted, IsFinished) = (true, false);
                (Current, Next) = (Next, default);
                return Current;
            }
            if (Source.MoveNext()) {
                (IsStarted, IsFinished) = (true, false);
                (Current, Next) = (Option.Create( Source.Current ), default);
                return Current;
            }
            (IsStarted, IsFinished) = (true, true);
            (Current, Next) = (default, default);
            return Current;
        }

        // Peek
        public Option<T> Peek() {
            if (Next.HasValue) {
                return Next;
            }
            if (Source.MoveNext()) {
                Next = Option.Create( Source.Current );
                return Next;
            }
            Next = default;
            return Next;
        }

        // Reset
        public void Reset() {
            Source.Reset();
            (IsStarted, IsFinished) = (false, false);
            (Current, Next) = (default, default);
        }

    }
    public static class PeekableEnumeratorExtensions {

        // Take/If
        public static Option<T> TakeIf<T>(this PeekableEnumerator<T> enumerator, Func<T, bool> predicate) {
            if (enumerator.Peek().TryGetValue( out var next ) && predicate( next )) {
                return enumerator.Take();
            }
            return default;
        }

        // Take/While
        public static IEnumerable<T> TakeWhile<T>(this PeekableEnumerator<T> enumerator, Func<T, bool> predicate) {
            // [true, true], false
            while (enumerator.Peek().TryGetValue( out var next ) && predicate( next )) {
                yield return enumerator.Take().Value;
            }
        }

    }
}

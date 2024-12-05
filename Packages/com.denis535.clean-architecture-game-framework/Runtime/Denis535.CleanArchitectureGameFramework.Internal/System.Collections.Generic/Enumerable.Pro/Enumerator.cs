#nullable enable
namespace System.Collections.Generic {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class EnumeratorExtensions {

        // Take
        public static Option<T> Take<T>(this IEnumerator<T> source) {
            if (source.MoveNext()) return Option.Create( source.Current );
            return default;
        }

        // As/Stateful
        public static StatefulEnumerator<T> AsStateful<T>(this IEnumerator<T> source) {
            return new StatefulEnumerator<T>( source );
        }
        // As/Peekable
        public static PeekableEnumerator<T> AsPeekable<T>(this IEnumerator<T> source) {
            return new PeekableEnumerator<T>( source );
        }

    }
}

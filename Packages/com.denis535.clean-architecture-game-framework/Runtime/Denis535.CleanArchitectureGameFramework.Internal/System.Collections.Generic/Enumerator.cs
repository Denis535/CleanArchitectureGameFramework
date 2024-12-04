#nullable enable
namespace System.Collections.Generic {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class EnumeratorExtensions {

        // Take
        public static Option<T> Take<T>(this IEnumerator<T> enumerator) {
            if (enumerator.MoveNext()) return Option.Create( enumerator.Current );
            return default;
        }

        // As/Stateful
        public static StatefulEnumerator<T> AsStateful<T>(this IEnumerator<T> enumerator) {
            return new StatefulEnumerator<T>( enumerator );
        }
        // As/Peekable
        public static PeekableEnumerator<T> AsPeekable<T>(this IEnumerator<T> enumerator) {
            return new PeekableEnumerator<T>( enumerator );
        }

    }
}

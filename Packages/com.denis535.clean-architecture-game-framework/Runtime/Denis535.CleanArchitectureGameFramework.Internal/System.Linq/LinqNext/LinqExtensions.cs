#nullable enable
namespace System.Linq {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class LinqExtensions {

        // Split
        // Split the items into segments (the separator is excluded)
        // Example: [false, false, false], true, [false, false, false]
        public static IEnumerable<T[]> Split<T>(this IEnumerable<T> source, Func<T, bool> separatorPredicate) {
            return source.FastSplit( separatorPredicate, i => i ).Select( i => i.ToArray() );
        }
        public static IEnumerable<TResult[]> Split<T, TResult>(this IEnumerable<T> source, Func<T, bool> separatorPredicate, Func<T, TResult> resultSelector) {
            return source.FastSplit( separatorPredicate, resultSelector ).Select( i => i.ToArray() );
        }
        public static IEnumerable<IList<TResult>> FastSplit<T, TResult>(this IEnumerable<T> source, Func<T, bool> separatorPredicate, Func<T, TResult> resultSelector) {
            var segment = new List<TResult>();
            foreach (var item in source) {
                if (separatorPredicate( item )) {
                    yield return segment;
                    segment.Clear();
                } else {
                    segment.Add( resultSelector( item ) );
                }
            }
            {
                yield return segment;
                segment.Clear();
            }
        }

        // Split/Before
        // Split the items into segments (the separator is included at the beginning of segment)
        // Example: [false, false, false], [true, false, false]
        public static IEnumerable<T[]> SplitBefore<T>(this IEnumerable<T> source, Func<T, bool> separatorPredicate) {
            return source.FastSplitBefore( separatorPredicate, i => i ).Select( i => i.ToArray() );
        }
        public static IEnumerable<TResult[]> SplitBefore<T, TResult>(this IEnumerable<T> source, Func<T, bool> separatorPredicate, Func<T, TResult> resultSelector) {
            return source.FastSplitBefore( separatorPredicate, resultSelector ).Select( i => i.ToArray() );
        }
        public static IEnumerable<IList<TResult>> FastSplitBefore<T, TResult>(this IEnumerable<T> source, Func<T, bool> separatorPredicate, Func<T, TResult> resultSelector) {
            var segment = new List<TResult>();
            foreach (var item in source) {
                if (separatorPredicate( item )) {
                    yield return segment;
                    segment.Clear();
                }
                segment.Add( resultSelector( item ) );
            }
            {
                yield return segment;
                segment.Clear();
            }
        }

        // Split/After
        // Split the items into segments (the separator is included at the end of segment)
        // Example: [false, false, true], [false, false, false]
        public static IEnumerable<T[]> SplitAfter<T>(this IEnumerable<T> source, Func<T, bool> separatorPredicate) {
            return source.FastSplitAfter( separatorPredicate, i => i ).Select( i => i.ToArray() );
        }
        public static IEnumerable<TResult[]> SplitAfter<T, TResult>(this IEnumerable<T> source, Func<T, bool> separatorPredicate, Func<T, TResult> resultSelector) {
            return source.FastSplitAfter( separatorPredicate, resultSelector ).Select( i => i.ToArray() );
        }
        public static IEnumerable<IList<TResult>> FastSplitAfter<T, TResult>(this IEnumerable<T> source, Func<T, bool> separatorPredicate, Func<T, TResult> resultSelector) {
            var segment = new List<TResult>();
            foreach (var item in source) {
                segment.Add( resultSelector( item ) );
                if (separatorPredicate( item )) {
                    yield return segment;
                    segment.Clear();
                }
            }
            {
                yield return segment;
                segment.Clear();
            }
        }

        // Slice
        // Slice the items into slices
        // Example: [true, true, true], [false, true, true]
        public static IEnumerable<T[]> Slice<T>(this IEnumerable<T> source, Func<T, IList<T>, bool> belongsToSlicePredicate) {
            return source.FastSlice( belongsToSlicePredicate, i => i ).Select( i => i.ToArray() );
        }
        public static IEnumerable<TResult[]> Slice<T, TResult>(this IEnumerable<T> source, Func<T, IList<TResult>, bool> belongsToSlicePredicate, Func<T, TResult> resultSelector) {
            return source.FastSlice( belongsToSlicePredicate, resultSelector ).Select( i => i.ToArray() );
        }
        public static IEnumerable<IList<TResult>> FastSlice<T, TResult>(this IEnumerable<T> source, Func<T, IList<TResult>, bool> belongsToSlicePredicate, Func<T, TResult> resultSelector) {
            using var source_enumerator = source.GetEnumerator();
            var slice = new List<TResult>();
            foreach (var item in source) {
                if (slice.Any() && !belongsToSlicePredicate( item, slice )) {
                    yield return slice;
                    slice.Clear();
                }
                slice.Add( resultSelector( item ) );
            }
            if (slice.Any()) {
                yield return slice;
                slice.Clear();
            }
        }

        // Unflatten
        // Unflatten the items into key-values pairs
        // Example: true: [false, false, false], true: [false, false, false]
        // Example: key: [value, value, value], key: [value, value, value]
        public static IEnumerable<(Option<T> Key, T[] Values)> Unflatten<T>(this IEnumerable<T> source, Func<T, bool> keyPredicate) {
            return source.FastUnflatten( keyPredicate, i => i, i => i ).Select( i => (i.Key, i.Values.ToArray()) );
        }
        public static IEnumerable<(Option<TKey> Key, TValue[] Values)> Unflatten<T, TKey, TValue>(this IEnumerable<T> source, Func<T, bool> keyPredicate, Func<T, TKey> keySelector, Func<T, TValue> valueSelector) {
            return source.FastUnflatten( keyPredicate, keySelector, valueSelector ).Select( i => (i.Key, i.Values.ToArray()) );
        }
        public static IEnumerable<(Option<TKey> Key, IList<TValue> Values)> FastUnflatten<T, TKey, TValue>(this IEnumerable<T> source, Func<T, bool> keyPredicate, Func<T, TKey> keySelector, Func<T, TValue> valueSelector) {
            using var source_enumerator = source.GetEnumerator();
            var key = default( Option<TKey> );
            var values = new List<TValue>();
            foreach (var item in source) {
                if (keyPredicate( item )) {
                    if (key.HasValue || values.Any()) {
                        yield return (key, values);
                        values.Clear();
                    }
                    key = Option.Create( keySelector( item ) );
                } else {
                    values.Add( valueSelector( item ) );
                }
            }
            if (key.HasValue || values.Any()) {
                yield return (key, values);
                values.Clear();
            }
        }

        // With/Prev
        public static IEnumerable<(T Value, Option<T> Prev)> WithPrev<T>(this IEnumerable<T> source) {
            var prev = default( Option<T> );
            foreach (var item in source) {
                yield return (item, prev);
                prev = Option.Create( item );
            }
        }
        // With/Next
        public static IEnumerable<(T Value, Option<T> Next)> WithNext<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            var value = source_enumerator.Take();
            var next = source_enumerator.Take();
            while (value.HasValue) {
                yield return (value.Value, next);
                (value, next) = (next, source_enumerator.Take());
            }
        }
        // With/Prev-Next
        public static IEnumerable<(T Value, Option<T> Prev, Option<T> Next)> WithPrevNext<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            var prev = default( Option<T> );
            var value = source_enumerator.Take();
            var next = source_enumerator.Take();
            while (value.HasValue) {
                yield return (value.Value, prev, next);
                (prev, value, next) = (value, next, source_enumerator.Take());
            }
        }

        // Tag/First
        public static IEnumerable<(T Value, bool IsFirst)> TagFirst<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            if (source_enumerator.Take().TryGetValue( out var first )) {
                yield return (first, true);
            }
            while (source_enumerator.Take().TryGetValue( out var value )) {
                yield return (value, false);
            }
        }
        // Tag/Last
        public static IEnumerable<(T Value, bool IsLast)> TagLast<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            var value = source_enumerator.Take();
            var next = source_enumerator.Take();
            while (value.HasValue) {
                yield return (value.Value, !next.HasValue);
                (value, next) = (next, source_enumerator.Take());
            }
        }
        // Tag/First-Last
        public static IEnumerable<(T Value, bool IsFirst, bool IsLast)> TagFirstLast<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            var value = source_enumerator.Take();
            var next = source_enumerator.Take();
            if (value.HasValue) {
                yield return (value.Value, true, !next.HasValue);
                (value, next) = (next, source_enumerator.Take());
            }
            while (value.HasValue) {
                yield return (value.Value, false, !next.HasValue);
                (value, next) = (next, source_enumerator.Take());
            }
        }

        // CompareTo
        public static void CompareTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, out T[] missing, out T[] extra) {
            var second_ = new LinkedList<T>( expected );
            extra = actual.Where( i => !second_.Remove( i ) ).ToArray();
            missing = second_.ToArray();
        }

    }
}

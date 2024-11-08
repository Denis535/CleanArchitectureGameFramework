#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class CSharp {

        // Chain
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T Chain<T>(this T value, Action<T> callback) {
            callback( value );
            return value;
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] ChainEach<T>(this T[] values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static List<T> ChainEach<T>(this List<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IEnumerable<T> ChainEach<T>(this IEnumerable<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyCollection<T> ChainEach<T>(this IReadOnlyCollection<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyList<T> ChainEach<T>(this IReadOnlyList<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ICollection<T> ChainEach<T>(this ICollection<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IList<T> ChainEach<T>(this IList<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }

        // Pipe
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TOutput Pipe<TInput, TOutput>(this TInput value, Converter<TInput, TOutput> callback) {
            return callback( value );
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TOutput[] PipeEach<TInput, TOutput>(this TInput[] values, Converter<TInput, TOutput> callback) {
            return Array.ConvertAll( values, callback );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static List<TOutput> PipeEach<TInput, TOutput>(this List<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IEnumerable<TOutput> PipeEach<TInput, TOutput>(this IEnumerable<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyCollection<TOutput> PipeEach<TInput, TOutput>(this IReadOnlyCollection<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyList<TOutput> PipeEach<TInput, TOutput>(this IReadOnlyList<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ICollection<TOutput> PipeEach<TInput, TOutput>(this ICollection<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IList<TOutput> PipeEach<TInput, TOutput>(this IList<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }

        // For
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void For<T>(this T value, Action<T> callback) {
            callback( value );
        }
        // ForEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this T[] values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this List<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        // ForEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IReadOnlyCollection<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IReadOnlyList<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this ICollection<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IList<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }

    }
}

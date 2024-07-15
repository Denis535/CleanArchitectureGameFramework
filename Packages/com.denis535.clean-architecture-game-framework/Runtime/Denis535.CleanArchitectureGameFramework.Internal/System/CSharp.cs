#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class CSharp {

        // Pipe
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T Pipe<T>(this T value, Action<T> callback) {
            callback( value );
            return value;
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] PipeEach<T>(this T[] values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static List<T> PipeEach<T>(this List<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyList<T> PipeEach<T>(this IReadOnlyList<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyCollection<T> PipeEach<T>(this IReadOnlyCollection<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }
        // PipeEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IEnumerable<T> PipeEach<T>(this IEnumerable<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
            return values;
        }

        // Chain
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TOutput Chain<TInput, TOutput>(this TInput value, Converter<TInput, TOutput> callback) {
            return callback( value );
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TOutput[] ChainEach<TInput, TOutput>(this TInput[] values, Converter<TInput, TOutput> callback) {
            return Array.ConvertAll( values, callback );
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static List<TOutput> ChainEach<TInput, TOutput>(this List<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyList<TOutput> ChainEach<TInput, TOutput>(this IReadOnlyList<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IReadOnlyCollection<TOutput> ChainEach<TInput, TOutput>(this IReadOnlyCollection<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) ).ToList();
        }
        // ChainEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IEnumerable<TOutput> ChainEach<TInput, TOutput>(this IEnumerable<TInput> values, Converter<TInput, TOutput> callback) {
            return values.Select( i => callback( i ) );
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
        // ForEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this List<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        // ForEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IReadOnlyList<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        // ForEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IReadOnlyCollection<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }
        // ForEach
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> callback) {
            foreach (var value in values) callback( value );
        }

    }
}

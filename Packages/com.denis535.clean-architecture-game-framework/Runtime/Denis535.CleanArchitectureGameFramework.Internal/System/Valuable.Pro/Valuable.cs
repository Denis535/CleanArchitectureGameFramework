#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class Valuable {

        public static IValuable<T> Create<T>() {
            return new Valuable_Empty<T>();
        }

        public static IValuable<T> Create<T>(this T value) {
            return new Valuable_Value<T>( value );
        }

        public static IValuable<TResult> Select<T, TResult>(this IValuable<T> valuable, Func<T, TResult> selector) {
            return new Valuable_ValueSelector<T, TResult>( valuable, selector );
        }

        public static IValuable<T> Where<T>(this IValuable<T> valuable, Func<T, bool> predicate) {
            return new Valuable_ValueFilter<T>( valuable, predicate );
        }

        public static T Value<T>(this IValuable<T> valuable) {
            if (valuable.TryGetValue( out var value )) {
                return value;
            }
            throw new InvalidOperationException( $"Valuable has no value" );
        }

        public static T? ValueOrDefault<T>(this IValuable<T> valuable) {
            if (valuable.TryGetValue( out var value )) {
                return value;
            }
            return default;
        }

    }
}

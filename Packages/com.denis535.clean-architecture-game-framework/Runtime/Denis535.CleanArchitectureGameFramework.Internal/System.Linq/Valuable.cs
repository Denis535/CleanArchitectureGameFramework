#nullable enable
namespace System.Linq {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static class Valuable {

        public static IValuable<T> Create<T>() {
            return new Valuable_Empty<T>();
        }

        public static IValuable<T> Create<T>(this T value) {
            return new Valuable_Value<T>( value );
        }

    }
    public interface IValuable<T> {

        bool TryGetValue([MaybeNullWhen( false )] out T value);

    }
    public static class IValuableExtensions {

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
    internal readonly struct Valuable_Empty<T> : IValuable<T> {

        //public Valuable_Empty() {
        //}

        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            value = default;
            return false;
        }

    }
    internal readonly struct Valuable_Value<T> : IValuable<T> {

        private readonly T value;

        public Valuable_Value(T value) {
            this.value = value;
        }

        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            value = this.value;
            return true;
        }

    }
    internal readonly struct Valuable_ValueSelector<T, TResult> : IValuable<TResult> {

        private readonly IValuable<T> target;
        private readonly Func<T, TResult> selector;

        public Valuable_ValueSelector(IValuable<T> target, Func<T, TResult> selector) {
            this.target = target;
            this.selector = selector;
        }

        public bool TryGetValue([MaybeNullWhen( false )] out TResult value) {
            if (target.TryGetValue( out var tmp )) {
                value = selector( tmp );
                return true;
            }
            value = default;
            return false;
        }

    }
    internal readonly struct Valuable_ValueFilter<T> : IValuable<T> {

        private readonly IValuable<T> target;
        private readonly Func<T, bool> predicate;

        public Valuable_ValueFilter(IValuable<T> target, Func<T, bool> predicate) {
            this.target = target;
            this.predicate = predicate;
        }

        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            if (target.TryGetValue( out var tmp )) {
                if (predicate( tmp )) {
                    value = tmp;
                    return true;
                }
            }
            value = default;
            return false;
        }

    }
}

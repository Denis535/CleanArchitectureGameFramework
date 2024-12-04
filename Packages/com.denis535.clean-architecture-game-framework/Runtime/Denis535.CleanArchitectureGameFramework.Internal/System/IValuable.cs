#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IValuable<T> {

        bool TryGetValue([MaybeNullWhen( false )] out T value);

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

        private readonly IValuable<T> source;
        private readonly Func<T, TResult> selector;

        public Valuable_ValueSelector(IValuable<T> source, Func<T, TResult> selector) {
            this.source = source;
            this.selector = selector;
        }

        public bool TryGetValue([MaybeNullWhen( false )] out TResult value) {
            if (source.TryGetValue( out var tmp )) {
                value = selector( tmp );
                return true;
            }
            value = default;
            return false;
        }

    }
    internal readonly struct Valuable_ValueFilter<T> : IValuable<T> {

        private readonly IValuable<T> source;
        private readonly Func<T, bool> predicate;

        public Valuable_ValueFilter(IValuable<T> source, Func<T, bool> predicate) {
            this.source = source;
            this.predicate = predicate;
        }

        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            if (source.TryGetValue( out var tmp )) {
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

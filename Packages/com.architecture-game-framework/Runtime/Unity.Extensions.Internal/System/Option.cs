#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public static class Option {

        // Create
        public static Option<T> Create<T>() {
            return new Option<T>();
        }
        public static Option<T> Create<T>(T value) {
            return new Option<T>( value );
        }
        public static Option<T> Create<T>(T? value) where T : struct {
            if (value.HasValue) return new Option<T>( value.Value );
            return default;
        }

        // Equals
        public static bool Equals<T>(Option<T> v1, Option<T> v2) {
            if (v1.HasValue && v2.HasValue) return EqualityComparer<T>.Default.Equals( v1.Value, v2.Value );
            return EqualityComparer<bool>.Default.Equals( v1.HasValue, v2.HasValue );
        }
        public static bool Equals<T>(Option<T> v1, T v2) {
            if (v1.HasValue) return EqualityComparer<T>.Default.Equals( v1.Value, v2 );
            return EqualityComparer<bool>.Default.Equals( v1.HasValue, true );
        }
        public static bool Equals<T>(Option<T> v1, T? v2) where T : struct {
            if (v1.HasValue && v2.HasValue) return EqualityComparer<T>.Default.Equals( v1.Value, v2.Value );
            return EqualityComparer<bool>.Default.Equals( v1.HasValue, v2.HasValue );
        }

        // Compare
        public static int Compare<T>(Option<T> v1, Option<T> v2) {
            if (v1.HasValue && v2.HasValue) return Comparer<T>.Default.Compare( v1.Value, v2.Value );
            return Comparer<bool>.Default.Compare( v1.HasValue, v2.HasValue );
        }
        public static int Compare<T>(Option<T> v1, T v2) {
            if (v1.HasValue) return Comparer<T>.Default.Compare( v1.Value, v2 );
            return Comparer<bool>.Default.Compare( v1.HasValue, true );
        }
        public static int Compare<T>(Option<T> v1, T? v2) where T : struct {
            if (v1.HasValue && v2.HasValue) return Comparer<T>.Default.Compare( v1.Value, v2.Value );
            return Comparer<bool>.Default.Compare( v1.HasValue, v2.HasValue );
        }

        // GetUnderlyingType
        public static Type? GetUnderlyingType(Type type) {
            if (GetUnboundType( type ) == typeof( Option<> )) return type.GetGenericArguments().First();
            return null;
        }

        // Helpers
        private static Type GetUnboundType(Type type) {
            if (type.IsGenericType) {
                return type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();
            } else {
                return type;
            }
        }

    }
    [Serializable]
    public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>, IComparable<Option<T>>, IComparable<T> {

        private readonly bool hasValue;
        private readonly T value;
        public static Option<T> Default => default;
        //[MemberMaybeNullWhen( false, nameof( ValueOrDefault ) )] // When HasValue == false: ValueOrDefault is always default
        //[MemberNotNullWhen( true, nameof( ValueOrDefault ) )]    // When HasValue == true:  ValueOrDefault can still be default, so it doesn't work right
        public bool HasValue => hasValue;
        public T Value => hasValue ? value : throw new InvalidOperationException( "Option must have value" );
        public T? ValueOrDefault => hasValue ? value : default;

        // Constructor
        public Option(T value) {
            this.hasValue = true;
            this.value = value;
        }

        // TryGetValue
        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            if (hasValue) {
                value = this.value;
                return true;
            }
            value = default;
            return false;
        }

        // Utils
        public bool Equals(Option<T> other) {
            return Option.Equals( this, other );
        }
        public bool Equals(T other) {
            return Option.Equals( this, other );
        }
        public int CompareTo(Option<T> other) {
            return Option.Compare( this, other );
        }
        public int CompareTo(T other) {
            return Option.Compare( this, other );
        }

        // Utils
        public override string ToString() {
            if (hasValue) return value?.ToString() ?? "Null";
            return "Nothing";
        }
        public override bool Equals(object? other) {
            if (other is Option<T> other_) return Option.Equals( this, other_ );
            return false;
        }
        public override int GetHashCode() {
            if (hasValue) return value?.GetHashCode() ?? 0;
            return 0;
        }

        // Utils
        public static implicit operator Option<T>(T value) {
            // This can lead to unexpected behavior:
            //var option = new Option<object>();
            //option = new Option<int>(); // This is equivalent to following:
            //option = new Option<object>( new Option<int>() ); // Now option<object> contains Option<int>
            return new Option<T>( value );
        }
        public static explicit operator T(Option<T> value) {
            return value.Value;
        }

        // Utils
        public static bool operator ==(Option<T> left, Option<T> right) {
            return Option.Equals( left, right );
        }
        public static bool operator !=(Option<T> left, Option<T> right) {
            return !Option.Equals( left, right );
        }

    }
}

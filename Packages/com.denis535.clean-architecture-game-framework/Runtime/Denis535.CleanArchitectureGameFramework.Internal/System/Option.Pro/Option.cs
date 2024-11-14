#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public static class Option {

        // Create
        public static Option<T> Create<T>(T value) {
            return new Option<T>( value );
        }
        public static Option<T> Create<T>(T? value) where T : struct {
            if (value.HasValue) return new Option<T>( value.Value );
            return default;
        }

        // Equals
        public static bool Equals<T>(Option<T> value, Option<T> other) {
            if (value.HasValue && other.HasValue) return EqualityComparer<T>.Default.Equals( value.Value, other.Value );
            return EqualityComparer<bool>.Default.Equals( value.HasValue, other.HasValue );
        }
        public static bool Equals<T>(Option<T> value, T other) {
            if (value.HasValue) return EqualityComparer<T>.Default.Equals( value.Value, other );
            return false;
        }
        public static bool Equals<T>(T value, Option<T> other) {
            if (other.HasValue) return EqualityComparer<T>.Default.Equals( value, other.Value );
            return false;
        }

        // Compare
        public static int Compare<T>(Option<T> value, Option<T> other) {
            if (value.HasValue && other.HasValue) return Comparer<T>.Default.Compare( value.Value, other.Value );
            return Comparer<bool>.Default.Compare( value.HasValue, other.HasValue );
        }
        public static int Compare<T>(Option<T> value, T other) {
            if (value.HasValue) return Comparer<T>.Default.Compare( value.Value, other );
            return Comparer<bool>.Default.Compare( false, true );
        }
        public static int Compare<T>(T value, Option<T> other) {
            if (other.HasValue) return Comparer<T>.Default.Compare( value, other.Value );
            return Comparer<bool>.Default.Compare( true, false );
        }

        // GetUnderlyingType
        public static Type GetUnderlyingType(Type type) {
            if (type == null) throw new ArgumentNullException( "type" );
            if (type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() != typeof( Option<> )) throw new ArgumentException( "Argument 'type' is invalid" );
            return type.GetGenericArguments()[ 0 ];
        }

    }
    [Serializable]
    public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>, IComparable<Option<T>>, IComparable<T> {

        private readonly bool hasValue;
        private readonly T value; // always is null/default when Option has no value

        // Value
        public bool HasValue => hasValue;
        public T Value => hasValue ? value : throw new InvalidOperationException( "Option has no value" );
        public T? ValueOrDefault => hasValue ? value : default; // always is null/default when Option has no value

        // Constructor
        //public Option() {
        //    this.hasValue = false;
        //    this.value = default!;
        //}
        internal Option(T value) {
            this.hasValue = true;
            this.value = value;
        }

        // TryGetValue
        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            if (HasValue) {
                value = this.Value;
                return true;
            }
            value = default;
            return false;
        }

        // Equals
        public bool Equals(Option<T> other) {
            return Option.Equals( this, other );
        }
        public bool Equals(T other) {
            return Option.Equals( this, other );
        }

        // CompareTo
        public int CompareTo(Option<T> other) {
            return Option.Compare( this, other );
        }
        public int CompareTo(T other) {
            return Option.Compare( this, other );
        }

        // Utils
        public override string ToString() {
            if (HasValue) return Value?.ToString() ?? "Null";
            return "Nothing";
        }
        public override bool Equals(object? other) {
            if (other is Option<T> other_) return Option.Equals( this, other_ );
            return false;
        }
        public override int GetHashCode() {
            if (HasValue) return Value?.GetHashCode() ?? 0;
            return 0;
        }

        // Utils
        //public static explicit operator Option<T>(T value) {
        //    return new Option<T>( value );
        //}
        //public static explicit operator Option<object?>(Option<T> value) {
        //    // Todo:    
        //    // How to write explicit cast operator with its own generic type???
        //    // Now I can cast only to non-generic types!!!
        //    // https://github.com/dotnet/csharplang/issues/813
        //    return new Option<object?>( value.hasValue, value.value );
        //}
        //public static explicit operator Option<T>(Option<object?> value) {
        //    return new Option<T>( value.hasValue, (T) value.value );
        //}

        // Utils
        public static bool operator ==(Option<T> left, Option<T> right) {
            return Option.Equals( left, right );
        }
        public static bool operator ==(Option<T> left, T right) {
            return Option.Equals( left, right );
        }
        public static bool operator ==(T left, Option<T> right) {
            return Option.Equals( left, right );
        }

        // Utils
        public static bool operator !=(Option<T> left, Option<T> right) {
            return !Option.Equals( left, right );
        }
        public static bool operator !=(Option<T> left, T right) {
            return !Option.Equals( left, right );
        }
        public static bool operator !=(T left, Option<T> right) {
            return !Option.Equals( left, right );
        }

    }
}

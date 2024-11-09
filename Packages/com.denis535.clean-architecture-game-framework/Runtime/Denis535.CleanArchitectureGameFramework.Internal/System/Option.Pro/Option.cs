#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public static class Option {

        // Equals
        public static bool Equals<T>(Option<T> v1, Option<T> v2) {
            if (v1.HasValue && v2.HasValue) return EqualityComparer<T>.Default.Equals( v1.Value, v2.Value );
            return EqualityComparer<bool>.Default.Equals( v1.HasValue, v2.HasValue );
        }
        public static bool Equals<T>(Option<T> v1, T v2) {
            if (v1.HasValue) return EqualityComparer<T>.Default.Equals( v1.Value, v2 );
            return false;
        }
        public static bool Equals<T>(T v1, Option<T> v2) {
            if (v2.HasValue) return EqualityComparer<T>.Default.Equals( v1, v2.Value );
            return false;
        }

        // Compare
        public static int Compare<T>(Option<T> v1, Option<T> v2) {
            if (v1.HasValue && v2.HasValue) return Comparer<T>.Default.Compare( v1.Value, v2.Value );
            return Comparer<bool>.Default.Compare( v1.HasValue, v2.HasValue );
        }
        public static int Compare<T>(Option<T> v1, T v2) {
            if (v1.HasValue) return Comparer<T>.Default.Compare( v1.Value, v2 );
            return Comparer<bool>.Default.Compare( false, true );
        }
        public static int Compare<T>(T v1, Option<T> v2) {
            if (v2.HasValue) return Comparer<T>.Default.Compare( v1, v2.Value );
            return Comparer<bool>.Default.Compare( true, false );
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
    // Option
    [Serializable]
    public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>, IComparable<Option<T>>, IComparable<T> {

        private readonly bool hasValue;
        private readonly T value; // Note: may be null/default if Option has no value

        // Value
        public bool HasValue => hasValue; // Note: Option can have null/default value
        public T Value => hasValue ? value : throw new InvalidOperationException( "Option has no value" ); // Note: therefore, null/default is also valid Option's value
        public T? ValueOrDefault => hasValue ? value : default; // always null/default if Option has no value

        // Constructor
        //public Option() { // C# 9.0 doesn't support it
        //    this.hasValue = false;
        //    this.value = default!;
        //}
        public Option(T value) {
            this.hasValue = true;
            this.value = value;
        }
        private Option(bool hasValue, T? value) {
            this.hasValue = hasValue;
            this.value = value!;
        }

        // TryGetValue
        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            if (HasValue) {
                value = this.value;
                return true;
            }
            value = default;
            return false;
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
        public bool Equals(Option<T> other) {
            return Option.Equals( this, other );
        }
        public bool Equals(T other) {
            return Option.Equals( this, other );
        }

        // Utils
        public int CompareTo(Option<T> other) {
            return Option.Compare( this, other );
        }
        public int CompareTo(T other) {
            return Option.Compare( this, other );
        }

        // Utils
        public static explicit operator Option<object?>(Option<T> value) {
            // todo: how to cast any generic option to any other generic option?
            // https://github.com/dotnet/csharplang/issues/813
            return new Option<object?>( value.hasValue, value.value );
        }
        public static explicit operator Option<T>(Option<object?> value) {
            // todo: how to cast any generic option to any other generic option?
            // https://github.com/dotnet/csharplang/issues/813
            return new Option<T>( value.hasValue, (T?) value.value );
        }

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
    // OptionExtensions
    public static class OptionExtensions {

        // AsOption
        public static Option<T> AsOption<T>(this T value) {
            return new Option<T>( value );
        }
        public static Option<T> AsOption<T>(this T? value) where T : struct {
            if (value.HasValue) return new Option<T>( value.Value );
            return default;
        }

    }
}

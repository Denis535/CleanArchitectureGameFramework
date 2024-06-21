#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    public static class StringExtensions {

        // TakeStartingWith
        public static string? TakeStartingWith(this string value, int index) {
            if (index != -1) return value.Substring( index );
            return null;
        }
        public static string? TakeStartingWith(this string value, char @string) {
            var i = value.IndexOf( @string );
            if (i != -1) return value.Substring( i );
            return null;
        }
        public static string? TakeStartingWith(this string value, string @string) {
            var i = value.IndexOf( @string );
            if (i != -1) return value.Substring( i );
            return null;
        }

        // TakeBeforeOf
        public static string? TakeBeforeOf(this string value, int index) {
            if (index != -1) return value.Substring( 0, index );
            return null;
        }
        public static string? TakeBeforeOf(this string value, char separator) {
            var i = value.IndexOf( separator );
            if (i != -1) return value.Substring( 0, i );
            return null;
        }
        public static string? TakeBeforeOf(this string value, string separator) {
            var i = value.IndexOf( separator );
            if (i != -1) return value.Substring( 0, i );
            return null;
        }

        // TakeAfterOf
        public static string? TakeAfterOf(this string value, int index) {
            if (index != -1) return value.Substring( index + 1 );
            return null;
        }
        public static string? TakeAfterOf(this string value, char separator) {
            var i = value.IndexOf( separator );
            if (i != -1) return value.Substring( i + 1 );
            return null;
        }
        public static string? TakeAfterOf(this string value, string separator) {
            var i = value.IndexOf( separator );
            if (i != -1) return value.Substring( i + separator.Length );
            return null;
        }

        // Format
        public static string Format(this string format, params object?[] args) {
            return string.Format( format, args );
        }
        public static string Format(this string format, CultureInfo culture, params object?[] args) {
            return string.Format( culture, format, args );
        }

        // Trim
        public static string Trim(this string value, string trim) {
            return value.TrimStart( trim ).TrimEnd( trim );
        }
        public static string TrimStart(this string value, string trim) {
            if (value.StartsWith( trim )) return value.Remove( 0, trim.Length );
            return value;
        }
        public static string TrimEnd(this string value, string trim) {
            if (value.EndsWith( trim )) return value.Remove( value.Length - trim.Length, trim.Length );
            return value;
        }

        // NullIf
        public static string? NullIfEmpty(this string value) {
            return !string.IsNullOrEmpty( value ) ? value : null;
        }
        public static string? NullIfWhiteSpace(this string value) {
            return !string.IsNullOrWhiteSpace( value ) ? value : null;
        }

    }
}

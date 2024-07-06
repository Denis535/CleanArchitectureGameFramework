#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class Array2 {

        // Create
        public static T[] Create<T>(params T[] array) {
            return array;
        }

        // IndexOf
        public static int IndexOf<T>(this T[] array, T value, int startIndex = 0) {
            return Array.IndexOf( array, value, startIndex );
        }
        public static int LastIndexOf<T>(this T[] array, T value, int startIndex = 0) {
            return Array.LastIndexOf( array, value, startIndex );
        }

        // Find
        public static T Find<T>(this T[] array, Predicate<T> match) {
            return Array.Find( array, match );
        }
        public static T FindLast<T>(this T[] array, Predicate<T> match) {
            return Array.FindLast( array, match );
        }
        public static T[] FindAll<T>(this T[] array, Predicate<T> match) {
            return Array.FindAll( array, match );
        }

        // FindIndex
        public static int FindIndex<T>(this T[] array, int startIndex, Predicate<T> match) {
            return Array.FindIndex( array, startIndex, match );
        }
        public static int FindLastIndex<T>(this T[] array, int startIndex, Predicate<T> match) {
            return Array.FindLastIndex( array, startIndex, match );
        }

        // NullIfEmpty
        public static T[]? NullIfEmpty<T>(this T[] array) {
            return array.Any() ? array : null;
        }

    }
}

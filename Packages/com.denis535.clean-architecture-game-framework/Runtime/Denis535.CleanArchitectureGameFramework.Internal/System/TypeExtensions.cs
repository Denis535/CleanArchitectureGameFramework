#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions {

        // Is
        public static bool Is(this Type type, Type general) {
            // Example:
            // Type<object> == Type<object>
            // Type<object> == Type<>
            return type.Iz( general );
        }
        // Is/BaseTypeOf
        public static bool IsBaseTypeOf(this Type type, Type derived) {
            // Example:
            // Base<object> is base type of Derived<object>
            // Base<>       is base type of Derived<object>
            return derived.IsChildOf( type );
        }
        public static bool IsAncesorOf(this Type type, Type derived) {
            // Example:
            // Base<object> is ancesor type of Derived<object>
            // Base<>       is ancesor type of Derived<object>
            return derived.IsDescendentOf( type );
        }
        // Is/ChildOf
        public static bool IsChildOf(this Type type, Type @base) {
            // Example:
            // Derived<object> is child of Base<object>
            // Derived<object> is child of Base<>
            return type.BaseType != null && type.BaseType.Iz( @base );
        }
        public static bool IsDescendentOf(this Type type, Type @base) {
            // Example:
            // Derived<object> is descendent of Base<object>
            // Derived<object> is descendent of Base<>
            while (type.BaseType != null) {
                if (type.BaseType.Iz( @base )) return true;
                type = type.BaseType;
            }
            return false;
        }
        // Is/ImplementationOf
        public static bool IsImplementationOf(this Type type, Type @interface) {
            return type.GetInterfaces().Any( i => i.Iz( @interface ) );
        }

        // Get/BaseType
        public static Type? GetBaseType(this Type type) {
            return type.BaseType;
        }
        public static IEnumerable<Type> GetAncesors(this Type type) {
            while (type.BaseType != null) {
                yield return type.BaseType;
                type = type.BaseType;
            }
        }
        // Get/Children
        public static IEnumerable<Type> GetChildren(this Type type, Assembly assembly) {
            foreach (var type_ in assembly.GetTypes()) {
                if (type_.IsChildOf( type )) yield return type_;
            }
        }
        public static IEnumerable<Type> GetDescendents(this Type type, Assembly assembly) {
            foreach (var type_ in assembly.GetTypes()) {
                if (type_.IsDescendentOf( type )) yield return type_;
            }
        }
        // Get/Implementations
        public static IEnumerable<Type> GetImplementations(this Type type, Assembly assembly) {
            foreach (var type_ in assembly.GetTypes()) {
                if (type_.IsImplementationOf( type )) yield return type_;
            }
        }

        // IsAssignableTo
        public static bool IsAssignableTo(this Type type, Type general) {
            return general.IsAssignableFrom( type );
        }

        // GetSimpleName
        public static string GetSimpleName(this Type type) {
            if (type.IsGenericType) {
                var i = type.Name.IndexOf( '`' );
                if (i != -1) return type.Name.Substring( 0, i );
            }
            return type.Name;
        }

        // GetGenericArgument
        public static Type? GetGenericArgument(this Type type, string name) {
            var keys = type.GetGenericTypeDefinition().GetGenericArguments();
            var values = type.GetGenericArguments();
            for (var i = 0; i < keys.Length; i++) {
                if (keys[ i ].Name == name) return values[ i ];
            }
            return null;
        }

        // GetGenericArguments
        public static IEnumerable<KeyValuePair<string, Type>> GetGenericArguments2(this Type type) {
            var keys = type.GetGenericTypeDefinition().GetGenericArguments();
            var values = type.GetGenericArguments();
            for (var i = 0; i < keys.Length; i++) {
                yield return new KeyValuePair<string, Type>( keys[ i ].Name, values[ i ] );
            }
        }

        // Helpers
        private static bool Iz(this Type type, Type general) {
            // Example:
            // Type<object> == Type<object>
            // Type<object> == Type<>
            if (type.IsGenericType && !type.IsGenericTypeDefinition) {
                if (general.IsGenericType && general.IsGenericTypeDefinition) {
                    return type.GetGenericTypeDefinition() == general;
                } else {
                    return type == general;
                }
            } else {
                return type == general;
            }
        }

    }
}

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
            return type.Iz( general );
        }
        // Is/BaseTypeOf
        public static bool IsBaseTypeOf(this Type type, Type derived) {
            return derived.BaseType != null && derived.BaseType.Iz( type );
        }
        public static bool IsAncesorOf(this Type type, Type derived) {
            while (derived.BaseType != null) {
                if (derived.BaseType.Iz( type )) return true;
                derived = derived.BaseType;
            }
            return false;
        }
        // Is/ChildOf
        public static bool IsChildOf(this Type type, Type @base) {
            return type.BaseType != null && type.BaseType.Iz( @base );
        }
        public static bool IsDescendentOf(this Type type, Type @base) {
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
        public static IEnumerable<Type> GetChildren(this Type type) => type.GetChildren( type.Assembly );
        public static IEnumerable<Type> GetChildren(this Type type, params Assembly[] assemblies) => assemblies.SelectMany( i => type.GetChildren( i ) );
        public static IEnumerable<Type> GetChildren(this Type type, Assembly assembly) {
            foreach (var type_ in assembly.GetTypes()) {
                if (type_.IsChildOf( type )) yield return type_;
            }
        }
        public static IEnumerable<Type> GetDescendents(this Type type) => type.GetDescendents( type.Assembly );
        public static IEnumerable<Type> GetDescendents(this Type type, params Assembly[] assemblies) => assemblies.SelectMany( i => type.GetDescendents( i ) );
        public static IEnumerable<Type> GetDescendents(this Type type, Assembly assembly) {
            foreach (var type_ in assembly.GetTypes()) {
                if (type_.IsDescendentOf( type )) yield return type_;
            }
        }
        // Get/Implementations
        public static IEnumerable<Type> GetImplementations(this Type type) => type.GetImplementations( type.Assembly );
        public static IEnumerable<Type> GetImplementations(this Type type, params Assembly[] assemblies) => assemblies.SelectMany( i => type.GetImplementations( i ) );
        public static IEnumerable<Type> GetImplementations(this Type type, Assembly assembly) {
            foreach (var type_ in assembly.GetTypes()) {
                if (type_.IsImplementationOf( type )) yield return type_;
            }
        }

        // IsAssignableFrom
        public static bool IsAssignableFrom(this Type type, Type specific) {
            return type.IsAssignableFrom( specific );
        }
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
            if (general.IsGenericTypeDefinition) {
                return type.SafeGetGenericTypeDefinition() == general;
            } else {
                return type == general;
            }
        }
        private static Type SafeGetGenericTypeDefinition(this Type type) {
            if (type.IsGenericType) return type.GetGenericTypeDefinition();
            return type;
        }

    }
}

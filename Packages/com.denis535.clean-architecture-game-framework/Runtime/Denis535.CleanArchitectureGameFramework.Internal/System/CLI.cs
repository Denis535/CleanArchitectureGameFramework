#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class CLI {

        // HasKey
        public static bool HasKey(string[] arguments, string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var i = Array.IndexOf( arguments, key );
            return i != -1;
        }

        // GetValue
        public static string? GetValue(string[] arguments, string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).FirstOrDefault();
            return null;
        }

        // GetValues
        public static string[]? GetValues(string[] arguments, string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).ToArray().NullIfEmpty();
            return null;
        }

        // GetKeyValues
        public static IEnumerable<(string? Key, string[] Values)> GetKeyValues(string[] arguments) {
            // Example:
            // val --key --key2 val --key3 val val val
            var key = (string?) null;
            var values = new List<string>();
            foreach (var arg in arguments) {
                if (arg.StartsWith( '-' )) {
                    if (key != null || values.Any()) yield return (key, values.ToArray());
                    key = arg;
                    values.Clear();
                } else {
                    values.Add( arg );
                }
            }
            if (key != null || values.Any()) yield return (key, values.ToArray());
        }

    }
}

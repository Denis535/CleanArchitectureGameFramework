#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    // Example:
    // --key --key2 val --key3 val val val
    public static class Environment2 {

        // GetKeyValues
        public static IEnumerable<(string? Key, string[] Values)> GetKeyValues() {
            var key = (string?) null;
            var values = new List<string>();
            foreach (var arg in Environment.GetCommandLineArgs()) {
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

        // HasKey
        public static bool HasKey(string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, key );
            return i != -1;
        }

        // GetValue
        public static string? GetValue(string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).FirstOrDefault();
            return null;
        }

        // GetValues
        public static string[]? GetValues(string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).ToArray().NullIfEmpty();
            return null;
        }

    }
}

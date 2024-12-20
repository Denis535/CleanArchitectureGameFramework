#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class CommandLineArguments {

        // HasKey
        public static bool HasKey(string[] arguments, string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var i = Array.IndexOf( arguments, key );
            return i != -1;
        }

        // GetValues
        public static IEnumerable<string>? GetValues(string[] arguments, string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) );
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
                    if (key != null || values.Any()) {
                        yield return (key, values.ToArray());
                    }
                    key = arg;
                    values.Clear();
                } else {
                    values.Add( arg );
                }
            }
            if (key != null || values.Any()) {
                yield return (key, values.ToArray());
            }
        }

    }
}

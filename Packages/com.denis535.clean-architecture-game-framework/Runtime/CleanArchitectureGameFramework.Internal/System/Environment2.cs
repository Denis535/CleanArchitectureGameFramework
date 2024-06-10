#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    // Example:
    // --key --key2 val --key3 val val val
    public static class Environment2 {

        public static bool HasArgument(string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, key );
            return i != -1;
        }

        public static string? GetArgument(string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).FirstOrDefault();
            return null;
        }

        public static string[]? GetArguments(string key) {
            Assert.Argument.Message( $"Key {key} must start with '-'" ).Valid( key.StartsWith( '-' ) );
            var arguments = Environment.GetCommandLineArgs();
            var i = Array.IndexOf( arguments, key );
            if (i != -1) return arguments.Skip( i + 1 ).TakeWhile( i => !i.StartsWith( '-' ) ).ToArray().NullIfEmpty();
            return null;
        }

    }
}

#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class Context {

        public static IDisposable Begin<T>(T value) {
            Assert.Operation.Message( $"Context {typeof( T )} must have no value" ).Valid( !Context<T>.Value.HasValue );
            return new ContextScope<T>( value );
        }

        public static bool HasValue<T>() {
            return Context<T>.Value.HasValue;
        }
        public static T GetValue<T>() {
            Assert.Operation.Message( $"Context {typeof( T )} must have value" ).Valid( Context<T>.Value.HasValue );
            return Context<T>.Value.Value;
        }

    }
    internal static class Context<T> {

        public static Option<T> Value { get; set; }

    }
    internal class ContextScope<T> : IDisposable {

        public ContextScope(T value) {
            Context<T>.Value = new Option<T>( value );
        }
        public void Dispose() {
            Context<T>.Value = default;
        }

    }
}

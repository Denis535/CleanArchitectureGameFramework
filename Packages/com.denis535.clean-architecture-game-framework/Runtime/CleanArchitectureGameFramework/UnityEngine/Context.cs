#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Context {

        public static Context<T> Begin<T>(T value) where T : notnull {
            return new Context<T>( value );
        }

        public static bool HasValue<T>() where T : notnull {
            return Context<T>.HasValue;
        }
        public static T GetValue<T>() where T : notnull {
            return Context<T>.Value;
        }

    }
    public struct Context<T> : IDisposable {

        private static Option<T> value;

        internal static bool HasValue => Context<T>.value.HasValue;
        internal static T Value {
            get {
                if (value.HasValue) return value.Value;
                throw Exceptions.Operation.InvalidOperationException( $"Context {typeof( T )} must have value" );
            }
        }

        internal Context(T value) {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            Assert.Operation.Message( $"Context {typeof( T )} must have no value" ).Valid( !Context<T>.value.HasValue );
            Context<T>.value = new Option<T>( value );
        }
        public void Dispose() {
            Context<T>.value = default;
        }

    }
}

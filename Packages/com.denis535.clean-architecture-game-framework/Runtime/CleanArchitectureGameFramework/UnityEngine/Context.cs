#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Context {

        // Begin
        public static IDisposable Begin<T>(object value) where T : notnull {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            return new Context<T>( value );
        }
        public static IDisposable Begin<T, TValue>(TValue value) where T : notnull where TValue : notnull {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            return new Context<T>( value );
        }

        // Has
        public static bool Has<T>() where T : notnull {
            var result = Context<T>.Value;
            return result != null;
        }

        // Get
        public static object Get<T>() where T : notnull {
            var result = Context<T>.Value;
            return result ?? throw Exceptions.Internal.Exception( $"ValueScope {typeof( T )} has no value" );
        }
        public static TValue Get<T, TValue>() where T : notnull {
            var result = (TValue?) Context<T>.Value;
            return result ?? throw Exceptions.Internal.Exception( $"ValueScope {typeof( T )} has no value" );
        }

    }
    internal class Context<T> : IDisposable where T : notnull {

        public static object? Value { get; private set; }

        // Constructor
        public Context(object value) {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            Assert.Operation.Message( $"Value {Value} must be null" ).Valid( Value == null );
            Value = value;
        }
        public void Dispose() {
            Assert.Operation.Message( $"Value {Value} must be non-null" ).Valid( Value != null );
            Value = null;
        }

    }
}

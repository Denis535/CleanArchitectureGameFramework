#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ValueScope {

        // Enter
        public static IDisposable Enter<T>(object value) where T : notnull {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            return new ValueScope<T>( value );
        }
        public static IDisposable Enter<T, TValue>(TValue value) where T : notnull where TValue : notnull {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            return new ValueScope<T>( value );
        }

        // Has
        public static bool Has<T>() where T : notnull {
            var result = ValueScope<T>.Value;
            return result != null;
        }

        // Get
        public static object Get<T>() where T : notnull {
            var result = ValueScope<T>.Value;
            return result ?? throw Exceptions.Internal.Exception( $"ValueScope {typeof( T )} has no value" );
        }
        public static TValue Get<T, TValue>() where T : notnull {
            var result = (TValue?) ValueScope<T>.Value;
            return result ?? throw Exceptions.Internal.Exception( $"ValueScope {typeof( T )} has no value" );
        }

    }
    internal class ValueScope<T> : IDisposable where T : notnull {

        public static object? Value { get; private set; }

        public ValueScope(object value) {
            Assert.Operation.Message( $"Value {Value} must be null" ).Valid( Value == null );
            Value = value;
        }
        public void Dispose() {
            Assert.Operation.Message( $"Value {Value} must be non-null" ).Valid( Value != null );
            Value = null;
        }

    }
}

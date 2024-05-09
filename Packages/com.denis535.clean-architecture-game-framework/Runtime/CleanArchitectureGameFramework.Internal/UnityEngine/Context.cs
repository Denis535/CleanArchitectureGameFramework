#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Context : IDisposable {

        private static object? value;

        // Value
        public static bool HasValue => value != null;
        public static object? Value => value ?? throw Exceptions.Operation.InvalidOperationException( $"Context has no value" );

        // Constructor
        private Context(object value) {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            Assert.Operation.Message( $"Value {Value} must be null" ).Valid( value == null );
            Context.value = value;
        }
        public void Dispose() {
            Assert.Operation.Message( $"Value {Value} must be non-null" ).Valid( value != null );
            Context.value = null;
        }

        // Begin
        public static Context Begin(object value) {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            return new Context( value );
        }
        public static Context Begin<TValue>(TValue value) where TValue : notnull {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            return new Context( value );
        }

    }
}

#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Context : IDisposable {

        private static object? value;

        // Value
        public static bool HasValue => Context.value != null;
        public static object Value => Context.value ?? throw Exceptions.Operation.InvalidOperationException( $"Context has no value" );

        // Constructor
        private Context(object value) {
            Context.value = value;
        }
        public void Dispose() {
            Context.value = null;
        }

        // Begin
        public static Context Begin(object value) {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            Assert.Operation.Message( $"Context must have no value" ).Valid( Context.value == null );
            return new Context( value );
        }

        // GetValue
        public static T GetValue<T>() {
            return (T?) Context.value ?? throw Exceptions.Operation.InvalidOperationException( $"Context has no value" );
        }

    }
}

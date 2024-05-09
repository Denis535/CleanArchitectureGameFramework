#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Context : IDisposable {

        // Value
        private static object? Value { get; set; }

        // Constructor
        private Context(object value) {
            Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
            Assert.Operation.Message( $"Value {Value} must be null" ).Valid( Value == null );
            Context.Value = value;
        }
        public void Dispose() {
            Assert.Operation.Message( $"Value {Value} must be non-null" ).Valid( Value != null );
            Context.Value = null;
        }

        // Begin
        public static Context Begin(object value) {
            return new Context( value );
        }

        // HasValue
        public static bool HasValue() {
            return Value != null;
        }
        // GetValue
        public static object GetValue() {
            return Value ?? throw Exceptions.Operation.InvalidOperationException( $"Context has no value" );
        }
        public static T GetValue<T>() {
            return (T?) Value ?? throw Exceptions.Operation.InvalidOperationException( $"Context has no value" );
        }

    }
}

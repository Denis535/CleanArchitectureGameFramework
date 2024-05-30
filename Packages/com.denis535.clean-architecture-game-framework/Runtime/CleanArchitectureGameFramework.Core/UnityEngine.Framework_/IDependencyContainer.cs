#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // todo: Make GetDependency and RequireDependency methods sealed. Now it doesn't work!!!
    public interface IDependencyContainer {

        // GetDependency
        public object? GetDependency(Type type, object? argument = null) {
            var option = GetValue( type, argument );
            return option.ValueOrDefault;
        }
        public T? GetDependency<T>(object? argument = null) {
            var option = GetValue( typeof( T ), argument );
            return (T?) option.ValueOrDefault;
        }

        // RequireDependency
        public object? RequireDependency(Type type, object? argument = null) {
            var option = GetValue( type, argument );
            Assert.Operation.Message( $"Dependency {type} ({argument}) was not found" ).Valid( option.HasValue );
            return option.Value;
        }
        public T RequireDependency<T>(object? argument = null) {
            var option = GetValue( typeof( T ), argument );
            Assert.Operation.Message( $"Dependency {typeof( T )} ({argument}) was not found" ).Valid( option.HasValue );
            return (T?) option.Value!;
        }

        // GetValue
        protected Option<object?> GetValue(Type type, object? argument);

    }
}

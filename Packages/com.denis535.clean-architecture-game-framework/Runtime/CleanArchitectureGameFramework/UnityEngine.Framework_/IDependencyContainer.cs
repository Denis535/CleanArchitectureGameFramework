#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    // todo: Make GetDependency and RequireDependency methods sealed. Now it doesn't work!!!
    public interface IDependencyContainer {

        private static IDependencyContainer? instance;

        protected internal static IDependencyContainer Instance {
            get {
                Assert.Operation.Message( $"DependencyContainer must be instantiated" ).Valid( instance != null );
                return instance;
            }
            set {
                Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
                Assert.Operation.Message( $"DependencyContainer is already instantiated" ).Valid( instance == null );
                instance = value;
            }
        }

        // GetDependency
        object? GetDependency(Type type, object? argument = null);
        T? GetDependency<T>(object? argument = null) where T : notnull {
            return (T?) GetDependency( typeof( T ), argument );
        }

        // RequireDependency
        object RequireDependency(Type type, object? argument = null) {
            var result = GetDependency( type, argument );
            Assert.Operation.Message( $"Object {type} ({argument}) was not found" ).Valid( result != null );
            return result;
        }
        T RequireDependency<T>(object? argument = null) where T : notnull {
            var result = GetDependency<T>( argument );
            Assert.Operation.Message( $"Object {typeof( T )} ({argument}) was not found" ).Valid( result != null );
            return result;
        }

    }
}

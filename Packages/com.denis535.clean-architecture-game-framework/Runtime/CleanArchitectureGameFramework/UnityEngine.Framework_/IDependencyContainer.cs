#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // todo: Make GetDependency and RequireDependency methods sealed. Now it doesn't work!!!
    public interface IDependencyContainer {

        private static IDependencyContainer? instance;

        public static IDependencyContainer Instance {
            get {
                Assert.Operation.Message( $"DependencyContainer must be instantiated" ).Valid( instance != null );
                return instance;
            }
            protected set {
                Assert.Argument.Message( $"Argument 'value' must be non-null" ).NotNull( value != null );
                Assert.Operation.Message( $"DependencyContainer is already instantiated" ).Valid( instance == null );
                instance = value;
            }
        }

        // GetDependency
        object? GetDependency(Type type, object? argument = null) {
            return GetObject( type, argument );
        }
        T? GetDependency<T>(object? argument = null) where T : notnull {
            return (T?) GetObject( typeof( T ), argument );
        }

        // RequireDependency
        object RequireDependency(Type type, object? argument = null) {
            var result = GetObject( type, argument );
            Assert.Operation.Message( $"Object {type} ({argument}) was not found" ).Valid( result != null );
            return result;
        }
        T RequireDependency<T>(object? argument = null) where T : notnull {
            var result = (T?) GetObject( typeof( T ), argument );
            Assert.Operation.Message( $"Object {typeof( T )} ({argument}) was not found" ).Valid( result != null );
            return result;
        }

        // GetObject
        object? GetObject(Type type, object? argument);

    }
}

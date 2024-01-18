#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    // todo: Make Resolve and TryResolve methods sealed. Now it doesn't work!!!
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

        // Resolve
        T Resolve<T>(object? argument) where T : notnull {
            var result = GetDependency<T>( argument );
            Assert.Operation.Message( $"Dependency {typeof( T )} ({argument}) was not resolved" ).Valid( result != null );
            return result;
        }
        object? Resolve(Type type, object? argument) {
            var result = GetDependency( type, argument );
            Assert.Operation.Message( $"Dependency {type} ({argument}) was not resolved" ).Valid( result != null );
            return result;
        }

        // TryResolve
        bool TryResolve<T>(object? argument, [NotNullWhen( true )] out T? result) where T : notnull {
            result = GetDependency<T>( argument );
            return result != null;
        }
        bool TryResolve(Type type, object? argument, [NotNullWhen( true )] out object? result) {
            result = GetDependency( type, argument );
            return result != null;
        }

        // GetDependency
        T? GetDependency<T>(object? argument) where T : notnull {
            return (T?) GetDependency( typeof( T ), argument );
        }
        object? GetDependency(Type type, object? argument);

    }
}

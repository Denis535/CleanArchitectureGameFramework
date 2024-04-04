#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class InitializationContext {

        public static IDisposable Begin<T>(object arguments) where T : notnull, Component {
            Assert.Argument.Message( $"Argument 'arguments' must be non-null" ).NotNull( arguments != null );
            return new InitializationContext<T>.Scope( arguments );
        }

        public static object GetArguments<T>() where T : notnull, Component {
            var result = (object?) InitializationContext<T>.Arguments;
            return result ?? throw Exceptions.Internal.Exception( $"InitializationContext {typeof( InitializationContext<T> )} has no arguments" );
        }
        public static TArguments GetArguments<T, TArguments>() where T : notnull, Component {
            var result = (TArguments?) InitializationContext<T>.Arguments;
            return result ?? throw Exceptions.Internal.Exception( $"InitializationContext {typeof( InitializationContext<T> )} has no arguments" );
        }

    }
    public static class InitializationContext<T> where T : notnull, Component {
        internal class Scope : IDisposable {
            public Scope(object arguments) {
                Arguments = arguments;
            }
            public void Dispose() {
                Arguments = null;
            }
        }

        public static object? Arguments { get; internal set; }

    }
    public static class InitializationContextExtensions {

        public static T AddComponent<T>(this GameObject gameObject, object arguments) where T : notnull, Component {
            using (InitializationContext.Begin<T>( arguments )) {
                return gameObject.AddComponent<T>();
            }
        }

    }
}

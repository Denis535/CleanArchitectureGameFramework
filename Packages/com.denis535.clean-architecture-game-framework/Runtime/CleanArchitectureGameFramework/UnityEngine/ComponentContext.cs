#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ComponentContext {

        public static IDisposable Enter<T>(object arguments) where T : notnull, Component {
            Assert.Argument.Message( $"Argument 'arguments' must be non-null" ).NotNull( arguments != null );
            return new ComponentContext<T>.Scope( arguments );
        }
        public static IDisposable Enter<T, TArguments>(TArguments arguments) where T : notnull, Component {
            Assert.Argument.Message( $"Argument 'arguments' must be non-null" ).NotNull( arguments != null );
            return new ComponentContext<T>.Scope( arguments );
        }

        public static object GetArguments<T>() where T : notnull, Component {
            var result = (object?) ComponentContext<T>.Arguments;
            return result ?? throw Exceptions.Internal.Exception( $"InitializationContext {typeof( ComponentContext<T> )} has no arguments" );
        }
        public static TArguments GetArguments<T, TArguments>() where T : notnull, Component {
            var result = (TArguments?) ComponentContext<T>.Arguments;
            return result ?? throw Exceptions.Internal.Exception( $"InitializationContext {typeof( ComponentContext<T> )} has no arguments" );
        }

    }
    internal static class ComponentContext<T> where T : notnull, Component {
        internal class Scope : IDisposable {

            public Scope(object arguments) {
                Assert.Argument.Message( $"Argument 'arguments' must be non-null" ).NotNull( arguments != null );
                Arguments = arguments;
            }

            public void Dispose() {
                Arguments = null;
            }

        }

        public static object? Arguments { get; private set; }

    }
    public static class ComponentContextExtensions {

        public static T AddComponent<T>(this GameObject gameObject, object arguments) where T : notnull, Component {
            using (ComponentContext.Enter<T>( arguments )) {
                return gameObject.AddComponent<T>();
            }
        }

    }
}

#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class Exceptions {
        public static class Internal {
            public static Exception Exception(FormattableString? message) => Exception<Exception>( message?.GetDisplayString() );
            public static NullReferenceException NullReference(FormattableString? message) => Exception<NullReferenceException>( message?.GetDisplayString() );
            public static NotSupportedException NotSupported(FormattableString? message) => Exception<NotSupportedException>( message?.GetDisplayString() );
            public static NotImplementedException NotImplemented(FormattableString? message) => Exception<NotImplementedException>( message?.GetDisplayString() );
        }

        // Helpers
        internal static string GetDisplayString(this FormattableString message) {
            var format = message.Format.Replace( "{", "'{" ).Replace( "}", "}'" );
            var arguments = message.GetArguments();
            for (var i = 0; i < arguments.Length; i++) {
                if (arguments[ i ] is IList list) {
                    arguments[ i ] = string.Join( ", ", list.Cast<object?>().ToArray() );
                } else
                if (arguments[ i ] is null) {
                    arguments[ i ] = "Null";
                }
            }
            return string.Format( format, arguments );
        }
        internal static T Exception<T>(string? message) where T : Exception {
            var type = typeof( T );
            var constructor = type.GetConstructor( new Type[] { typeof( string ), typeof( Exception ) } );
            return (T) constructor.Invoke( new object?[] { message, null } );
        }

    }
    // ObjectNotInitializedException
    public class ObjectNotInitializedException : InvalidOperationException {
        public ObjectNotInitializedException(string message) : base( message ) {
        }
        public ObjectNotInitializedException(string message, Exception? innerException) : base( message, innerException ) {
        }
    }
}

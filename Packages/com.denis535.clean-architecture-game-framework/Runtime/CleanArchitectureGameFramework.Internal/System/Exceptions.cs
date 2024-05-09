#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Exceptions {
        // Argument
        public static class Argument {
            public static ArgumentException ArgumentException(FormattableString? message) => GetException<ArgumentException>( message );
            public static ArgumentOutOfRangeException ArgumentOutOfRangeException(FormattableString? message) => GetException<ArgumentOutOfRangeException>( message );
            public static ArgumentNullException ArgumentNullException(FormattableString? message) => GetException<ArgumentNullException>( message );
        }
        // Operation
        public static class Operation {
            public static InvalidOperationException InvalidOperationException(FormattableString? message) => GetException<InvalidOperationException>( message );
        }
        // Object
        public static class Object {
            public static ObjectNotInitializedException ObjectNotInitializedException(FormattableString? message) => GetException<ObjectNotInitializedException>( message );
            public static ObjectDisposedException ObjectDisposedException(FormattableString? message) => GetException<ObjectDisposedException>( message );
        }
        // Internal
        public static class Internal {
            public static Exception Exception(FormattableString? message) => GetException<Exception>( message );
            public static NullReferenceException NullReference(FormattableString? message) => GetException<NullReferenceException>( message );
            public static NotSupportedException NotSupported(FormattableString? message) => GetException<NotSupportedException>( message );
            public static NotImplementedException NotImplemented(FormattableString? message) => GetException<NotImplementedException>( message );
        }
    }
    public static partial class Exceptions {

        public static Func<FormattableString?, string?> GetMessageStringDelegate = GetMessageString;
        public static Func<object?, string> GetArgumentStringDelegate = GetArgumentString;
        public static Func<Type, string?, Exception> GetExceptionDelegate = GetException;

        // GetException
        internal static T GetException<T>(FormattableString? message) where T : Exception {
            return GetException<T>( GetMessageStringDelegate( message ) );
        }
        internal static T GetException<T>(string? message) where T : Exception {
            return (T) GetExceptionDelegate( typeof( T ), message );
        }

        // Helpers
        private static string? GetMessageString(this FormattableString? message) {
            if (message != null) {
                var format = message.Format.Replace( "{", "'{" ).Replace( "}", "}'" );
                var arguments = message.GetArguments();
                for (var i = 0; i < arguments.Length; i++) {
                    arguments[ i ] = GetArgumentStringDelegate( arguments[ i ] );
                }
                return string.Format( format, arguments );
            }
            return null;
        }
        private static string GetArgumentString(object? argument) {
            if (argument is ICollection collection) {
                return string.Join( ", ", collection.Cast<object?>().Select( GetArgumentString ) );
            }
            if (argument is IList list) {
                return string.Join( ", ", list.Cast<object?>().Select( GetArgumentString ) );
            }
            if (argument is null) {
                return "Null";
            }
            return argument.ToString();
        }
        private static Exception GetException(Type type, string? message) {
            var constructor = type.GetConstructor( new Type[] { typeof( string ), typeof( Exception ) } );
            return (Exception) constructor.Invoke( new object?[] { message, null } );
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

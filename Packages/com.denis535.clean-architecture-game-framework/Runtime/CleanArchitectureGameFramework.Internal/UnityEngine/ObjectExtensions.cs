#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ObjectExtensions {

        // ThrowIfInvalid
        public static void ThrowIfInvalid<T>(this T @object) where T : Object {
            Assert.Argument.Message( $"Argument '@object' ({typeof( T )}) must be non-null" ).NotNull( @object is not null );
            if (@object is MonoBehaviour object_MonoBehaviour) {
                Assert.Operation.Message( $"Object {object_MonoBehaviour} must be awakened" ).Ready( object_MonoBehaviour.didAwake );
            }
            Assert.Operation.Message( $"Object {@object} must not be disposed" ).NotDisposed( @object );
        }

        // IfValid
        public static T IfValid<T>(this T @object) where T : Object {
            @object.ThrowIfInvalid();
            return @object;
        }

    }
}

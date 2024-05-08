#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ObjectExtensions {

        // Validate
        public static T Validate<T>(this T @object) where T : Object {
            Assert.Argument.Message( $"Argument '@object' ({typeof( T )}) must be non-null" ).NotNull( @object is not null );
            if (@object is MonoBehaviour object_MonoBehaviour) {
                Assert.Object.Message( $"Object {object_MonoBehaviour} must be awakened" ).Initialized( object_MonoBehaviour.didAwake );
            }
            Assert.Object.Message( $"Object {@object} must be alive" ).Alive( @object );
            return @object;
        }

    }
}

#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ObjectExtensions {

        // Check
        public static void Check(this Object @object) {
            if (@object is MonoBehaviour object_MonoBehaviour) {
                Assert.Object.Message( $"Object {object_MonoBehaviour} must be awakened" ).Initialized( object_MonoBehaviour.didAwake );
            }
            Assert.Object.Message( $"Object {@object} must be alive" ).Alive( @object );
        }

    }
}

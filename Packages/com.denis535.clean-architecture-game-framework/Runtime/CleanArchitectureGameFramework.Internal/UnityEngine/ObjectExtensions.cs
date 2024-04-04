#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ObjectExtensions {

        // Check
        public static void Check(this Object @object) {
            Assert.Object.Message( $"Object {@object} must be alive" ).Alive( @object );
        }

    }
}

#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Object2 {

        // Require/Object
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireAnyObjectByType<T>(FindObjectsInactive findObjectsInactive) where T : Object {
            var result = Object.FindAnyObjectByType<T>( findObjectsInactive );
            Assert.Operation.Message( $"Object {typeof( T )} was not found" ).Valid( result != null );
            return result;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireFirstObjectByType<T>(FindObjectsInactive findObjectsInactive) where T : Object {
            var result = Object.FindFirstObjectByType<T>( findObjectsInactive );
            Assert.Operation.Message( $"Object {typeof( T )} was not found" ).Valid( result != null );
            return result;
        }

        // Require/Objects
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireObjectsByType<T>(FindObjectsInactive findObjectsInactive, FindObjectsSortMode sortMode) where T : Object {
            var result = Object.FindObjectsByType<T>( findObjectsInactive, sortMode ).NullIfEmpty();
            Assert.Operation.Message( $"Objects {typeof( T )} was not found" ).Valid( result != null );
            return result;
        }

    }
}

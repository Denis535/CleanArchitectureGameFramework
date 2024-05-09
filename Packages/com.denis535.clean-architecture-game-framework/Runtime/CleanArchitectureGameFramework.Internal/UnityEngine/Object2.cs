#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Object2 {

        // Instantiate
        public static T Instantiate<T>(T original, object? arguments) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( original );
            }
            return Object.Instantiate( original );
        }
        public static T Instantiate<T>(T original, Transform? parent, object? arguments) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( original, parent );
            }
            return Object.Instantiate( original, parent );
        }
        public static T Instantiate<T>(T original, Transform? parent, bool instantiateInWorldSpace, object? arguments) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( original, parent, instantiateInWorldSpace );
            }
            return Object.Instantiate( original, parent, instantiateInWorldSpace );
        }
        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, object? arguments) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( original, position, rotation );
            }
            return Object.Instantiate( original, position, rotation );
        }
        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform? parent, object? arguments) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( original, position, rotation, parent );
            }
            return Object.Instantiate( original, position, rotation, parent );
        }

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

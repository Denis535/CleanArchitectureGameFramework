#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Object2 {

        // Instantiate
        public static Object Instantiate(Object @object, object? arguments) {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object );
            }
            return Object.Instantiate( @object );
        }
        public static Object Instantiate(Object @object, object? arguments, Transform? parent) {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, parent );
            }
            return Object.Instantiate( @object, parent );
        }
        public static Object Instantiate(Object @object, object? arguments, Transform? parent, bool instantiateInWorldSpace) {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, parent, instantiateInWorldSpace );
            }
            return Object.Instantiate( @object, parent, instantiateInWorldSpace );
        }
        public static Object Instantiate(Object @object, object? arguments, Vector3 position, Quaternion rotation) {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, position, rotation );
            }
            return Object.Instantiate( @object, position, rotation );
        }
        public static Object Instantiate(Object @object, object? arguments, Vector3 position, Quaternion rotation, Transform? parent) {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, position, rotation, parent );
            }
            return Object.Instantiate( @object, position, rotation, parent );
        }

        // Instantiate
        public static T Instantiate<T>(T @object, object? arguments) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object );
            }
            return Object.Instantiate( @object );
        }
        public static T Instantiate<T>(T @object, object? arguments, Transform? parent) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, parent );
            }
            return Object.Instantiate( @object, parent );
        }
        public static T Instantiate<T>(T @object, object? arguments, Transform? parent, bool instantiateInWorldSpace) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, parent, instantiateInWorldSpace );
            }
            return Object.Instantiate( @object, parent, instantiateInWorldSpace );
        }
        public static T Instantiate<T>(T @object, object? arguments, Vector3 position, Quaternion rotation) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, position, rotation );
            }
            return Object.Instantiate( @object, position, rotation );
        }
        public static T Instantiate<T>(T @object, object? arguments, Vector3 position, Quaternion rotation, Transform? parent) where T : Object {
            if (arguments != null) {
                using (Context.Begin( arguments )) return Object.Instantiate( @object, position, rotation, parent );
            }
            return Object.Instantiate( @object, position, rotation, parent );
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

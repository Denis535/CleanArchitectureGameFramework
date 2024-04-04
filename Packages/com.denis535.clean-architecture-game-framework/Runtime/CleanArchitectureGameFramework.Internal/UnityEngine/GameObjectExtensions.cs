#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class GameObjectExtensions {

        // Require/Component
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireComponent<T>(this GameObject gameObject) {
            var result = gameObject.GetComponent<T>();
            Assert.Operation.Message( $"Component {typeof( T )} was not found" ).Valid( result != null );
            return result;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireComponentInChildren<T>(this GameObject gameObject, bool includeInactive = false) {
            var result = gameObject.GetComponentInChildren<T>( includeInactive );
            Assert.Operation.Message( $"Component {typeof( T )} (in children) was not found" ).Valid( result != null );
            return result;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireComponentInParent<T>(this GameObject gameObject, bool includeInactive = false) {
            var result = gameObject.GetComponentInParent<T>( includeInactive );
            Assert.Operation.Message( $"Component {typeof( T )} (in parent) was not found" ).Valid( result != null );
            return result;
        }

        // Require/Components
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireComponents<T>(this GameObject gameObject) {
            var result = gameObject.GetComponents<T>().NullIfEmpty();
            Assert.Operation.Message( $"Components {typeof( T )} was not found" ).Valid( result != null );
            return result;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireComponentsInChildren<T>(this GameObject gameObject, bool includeInactive = false) {
            var result = gameObject.GetComponentsInChildren<T>( includeInactive ).NullIfEmpty();
            Assert.Operation.Message( $"Components {typeof( T )} (in children) was not found" ).Valid( result != null );
            return result;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireComponentsInParent<T>(this GameObject gameObject, bool includeInactive = false) {
            var result = gameObject.GetComponentsInParent<T>( includeInactive ).NullIfEmpty();
            Assert.Operation.Message( $"Components {typeof( T )} (in parent) was not found" ).Valid( result != null );
            return result;
        }

    }
}

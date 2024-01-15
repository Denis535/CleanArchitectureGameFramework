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
            return gameObject.GetComponent<T>() ?? throw Exceptions.Internal.Exception( $"Component {typeof( T )} was not found" );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireComponentInChildren<T>(this GameObject gameObject, bool includeInactive = false) {
            return gameObject.GetComponentInChildren<T>( includeInactive ) ?? throw Exceptions.Internal.Exception( $"Component {typeof( T )} (in children) was not found" );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T RequireComponentInParent<T>(this GameObject gameObject, bool includeInactive = false) {
            return gameObject.GetComponentInParent<T>( includeInactive ) ?? throw Exceptions.Internal.Exception( $"Component {typeof( T )} (in parent) was not found" );
        }

        // Require/Components
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireComponents<T>(this GameObject gameObject) {
            return gameObject.GetComponents<T>().NullIfEmpty() ?? throw Exceptions.Internal.Exception( $"Components {typeof( T )} was not found" );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireComponentsInChildren<T>(this GameObject gameObject, bool includeInactive = false) {
            return gameObject.GetComponentsInChildren<T>( includeInactive ).NullIfEmpty() ?? throw Exceptions.Internal.Exception( $"Components {typeof( T )} (in children) was not found" );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T[] RequireComponentsInParent<T>(this GameObject gameObject, bool includeInactive = false) {
            return gameObject.GetComponentsInParent<T>( includeInactive ).NullIfEmpty() ?? throw Exceptions.Internal.Exception( $"Components {typeof( T )} (in parent) was not found" );
        }

    }
}

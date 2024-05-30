#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public abstract class UIThemeBase : Disposable {

        // Constructor
        public UIThemeBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        protected static void Shuffle<T>(T[] array) {
            for (int i = 0, j = array.Length; i < array.Length; i++, j--) {
                var rnd = i + UnityEngine.Random.Range( 0, j );
                (array[ i ], array[ rnd ]) = (array[ rnd ], array[ i ]);
            }
        }
        protected static T[] GetShuffled<T>(T[] array) {
            var random = new System.Random();
            var result = array.ToArray();
            for (int i = 0, j = result.Length; i < result.Length; i++, j--) {
                var rnd = i + random.Next( 0, j );
                (result[ i ], result[ rnd ]) = (result[ rnd ], result[ i ]);
            }
            return result;
        }
        protected static T GetNextValue<T>(T[] array, T? value) {
            var index = array.IndexOf( value );
            if (index != -1) {
                index = (index + 1) % array.Length;
                return array[ index ];
            }
            return array[ 0 ];
        }
        protected static T GetRandomValue<T>(T[] array, T? value) {
            var index = UnityEngine.Random.Range( 0, array.Length );
            if (index != -1) {
                if (ReferenceEquals( array[ index ], value ) && array.Length > 1) {
                    return GetRandomValue( array, value );
                }
                return array[ index ];
            }
            return array[ 0 ];
        }

    }
}

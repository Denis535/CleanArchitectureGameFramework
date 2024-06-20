#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public abstract class UIThemeBase2 : UIThemeBase {

        // System
        protected IDependencyContainer Container { get; }
        protected AudioSource AudioSource { get; }

        // Constructor
        public UIThemeBase2(IDependencyContainer container, AudioSource audioSource) {
            Container = container;
            AudioSource = audioSource;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        protected static void Play(AudioSource source, AudioClip clip) {
            Assert.Operation.Message( $"AudioSource {source} must have no clip" ).Valid( source.clip == null );
            source.clip = clip;
            source.Play();
        }
        protected static void Stop(AudioSource source) {
            Assert.Operation.Message( $"AudioSource {source} must have clip" ).Valid( source.clip != null );
            source.Stop();
            source.clip = null;
        }
        protected static void SetPaused(AudioSource source, bool value) {
            if (value) {
                source.Pause();
            } else {
                source.UnPause();
            }
        }
        protected static bool IsPlaying(AudioSource source) {
            return source.clip != null && source.time < source.clip.length;
        }
        protected static bool IsCompleted(AudioSource source) {
            return !IsPlaying( source );
        }
        // Helpers
        protected static void Shuffle<T>(T[] array) {
            for (int i = 0, j = array.Length; i < array.Length; i++, j--) {
                var rnd = i + UnityEngine.Random.Range( 0, j );
                (array[ i ], array[ rnd ]) = (array[ rnd ], array[ i ]);
            }
        }
        protected static T[] GetShuffled<T>(T[] array) {
            var result = array.ToArray();
            Shuffle( result );
            return result;
        }
        protected static T GetNext<T>(T[] array, T? value) {
            var index = array.IndexOf( value );
            if (index != -1) {
                index = (index + 1) % array.Length;
                return array[ index ];
            }
            return array[ 0 ];
        }
        protected static T GetRandom<T>(T[] array, T? value) {
            var index = UnityEngine.Random.Range( 0, array.Length );
            if (index != -1) {
                if (ReferenceEquals( array[ index ], value ) && array.Length > 1) {
                    return GetRandom( array, value );
                }
                return array[ index ];
            }
            return array[ 0 ];
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIThemeBase2 : UIThemeBase {

        // System
        protected IDependencyContainer Container { get; }
        // AudioSource
        protected AudioSource AudioSource { get; }
        // IsPlaying
        protected bool IsPlaying {
            get => AudioSource.clip != null;
        }
        protected bool Mute {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                return AudioSource.mute;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                AudioSource.mute = value;
            }
        }
        protected float Volume {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                return AudioSource.volume;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                AudioSource.volume = value;
            }
        }
        protected float Pitch {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                return AudioSource.pitch;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                AudioSource.pitch = value;
            }
        }
        protected bool IsPaused {
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                if (value) {
                    AudioSource.Pause();
                } else {
                    AudioSource.UnPause();
                }
            }
        }
        protected bool IsCompleted {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                return !AudioSource.isPlaying && AudioSource.time == AudioSource.clip.length;
            }
        }

        // Constructor
        public UIThemeBase2(IDependencyContainer container, AudioSource audioSource) {
            Container = container;
            AudioSource = audioSource;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Play
        protected void Play(AudioClip clip) {
            Assert.Operation.Message( $"Theme {this} must be non-playing" ).Valid( AudioSource.clip == null );
            AudioSource.clip = clip;
            AudioSource.Play();
        }
        protected void Stop() {
            Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
            AudioSource.Stop();
            AudioSource.clip = null;
        }

        // Helpers
        protected static T[] Shuffle<T>(T[] array) {
            for (int i = 0, j = array.Length; i < array.Length; i++, j--) {
                var rnd = i + UnityEngine.Random.Range( 0, j );
                (array[ i ], array[ rnd ]) = (array[ rnd ], array[ i ]);
            }
            return array;
        }
        protected static T GetNext<T>(T[] array, T? value) {
            var index = Array.IndexOf( array, value );
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

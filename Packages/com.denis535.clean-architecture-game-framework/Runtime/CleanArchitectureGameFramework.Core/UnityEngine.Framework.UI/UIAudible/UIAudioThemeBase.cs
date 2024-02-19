#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [DefaultExecutionOrder( ScriptExecutionOrders.UIAudioTheme )]
    public abstract class UIAudioThemeBase : MonoBehaviour {

        // Globals
        protected AudioSource AudioSource { get; set; } = default!;
        // Clip
        public AudioClip? Clip => AudioSource.clip;
        public bool IsPlaying { get; protected set; }
        public bool IsPaused { get; protected set; }
        public bool IsUnPaused => !IsPaused;
        public float Time { get => AudioSource.time; set => AudioSource.time = value; }
        public float Volume { get => AudioSource.volume; set => AudioSource.volume = value; }
        public bool Mute { get => AudioSource.mute; set => AudioSource.mute = value; }

        // Awake
        public void Awake() {
            AudioSource = gameObject.RequireComponentInChildren<AudioSource>();
        }
        public void OnDestroy() {
        }

        // Play
        protected void SetPlaying(bool value) {
            if (value) {
                Play( AudioSource.clip );
            } else {
                Stop();
            }
        }
        protected void Play(AudioClip clip) {
            AudioSource.clip = clip;
            AudioSource.Play();
            IsPlaying = true;
        }
        protected void Stop() {
            AudioSource.Stop();
            AudioSource.clip = null;
            IsPlaying = false;
        }

        // Pause
        protected void SetPaused(bool value) {
            if (value) {
                Pause();
            } else {
                UnPause();
            }
        }
        protected void Pause() {
            if (IsPaused) return;
            AudioSource.Pause();
            IsPaused = true;
        }
        protected void UnPause() {
            if (!IsPaused) return;
            AudioSource.UnPause();
            IsPaused = false;
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

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIPlayListBase : StateBase<UIPlayListBase>, IDisposable {

        // System
        public bool IsDisposed { get; private set; }
        // Context
        protected UIThemeBase Context { get; }
        // IsPlaying
        protected bool IsPlaying {
            get => Context.AudioSource.clip != null;
        }
        protected bool IsCompleted {
            get {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                return !Context.AudioSource.isPlaying && Context.AudioSource.time == Context.AudioSource.clip.length;
            }
        }
        protected bool IsPaused {
            set {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                if (value) {
                    Context.AudioSource.Pause();
                } else {
                    Context.AudioSource.UnPause();
                }
            }
        }
        protected bool Mute {
            get {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                return Context.AudioSource.mute;
            }
            set {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                Context.AudioSource.mute = value;
            }
        }
        protected float Volume {
            get {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                return Context.AudioSource.volume;
            }
            set {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                Context.AudioSource.volume = value;
            }
        }
        protected float Pitch {
            get {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                return Context.AudioSource.pitch;
            }
            set {
                Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
                Context.AudioSource.pitch = value;
            }
        }

        // Constructor
        public UIPlayListBase(UIThemeBase context) {
            Context = context;
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"PlayList {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"PlayList {this} must be inactive" ).Valid( State is StateBase.State_.Inactive );
            IsDisposed = true;
        }

        // Play
        protected void Play(AudioClip clip) {
            Assert.Operation.Message( $"Theme {Context} must be non-playing" ).Valid( Context.AudioSource.clip == null );
            Context.AudioSource.clip = clip;
            Context.AudioSource.Play();
        }
        protected void Stop() {
            Assert.Operation.Message( $"Theme {Context} must be playing" ).Valid( Context.AudioSource.clip != null );
            Context.AudioSource.Stop();
            Context.AudioSource.clip = null;
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

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIThemeBase : DisposableBase {

        // AudioSource
        protected AudioSource AudioSource { get; }
        // Stateful
        private Stateful<UIPlayListBase> Stateful { get; } = new Stateful<UIPlayListBase>();
        // PlayList
        protected UIPlayListBase? PlayList {
            get {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return Stateful.State;
            }
        }
        // IsPlaying
        protected internal bool IsPlaying {
            get {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return AudioSource.clip != null;
            }
        }
        protected internal bool IsCompleted {
            get {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                return !AudioSource.isPlaying && AudioSource.time == AudioSource.clip.length;
            }
        }
        protected internal bool IsPaused {
            set {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                if (value) {
                    AudioSource.Pause();
                } else {
                    AudioSource.UnPause();
                }
            }
        }
        protected internal bool Mute {
            get {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                return AudioSource.mute;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                AudioSource.mute = value;
            }
        }
        protected internal float Volume {
            get {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                return AudioSource.volume;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                AudioSource.volume = value;
            }
        }
        protected internal float Pitch {
            get {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                return AudioSource.pitch;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( IsPlaying );
                AudioSource.pitch = value;
            }
        }

        // Constructor
        public UIThemeBase(AudioSource audioSource) {
            AudioSource = audioSource;
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Theme {this} must be non-playing" ).Valid( !IsPlaying );
            Assert.Operation.Message( $"Theme {this} must have no play list" ).Valid( PlayList == null );
            base.Dispose();
        }

        // SetPlayList
        protected virtual void SetPlayList(UIPlayListBase? playList, object? argument = null) {
            if (playList != null) {
                Assert.Argument.Message( $"Argument 'playList' ({playList}) must be non-disposed" ).Valid( !playList.IsDisposed );
                Assert.Argument.Message( $"Argument 'playList' ({playList}) must be inactive" ).Valid( playList.State is StateBase.State_.Inactive );
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Stateful.SetState( playList, argument );
            } else {
                Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Stateful.SetState( null, argument );
                Assert.Operation.Message( $"Theme {this} must be non-playing" ).Valid( !IsPlaying );
            }
        }

        // Play
        internal void Play(AudioClip clip) {
            Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Theme {this} must be non-playing" ).Valid( !IsPlaying );
            AudioSource.clip = clip;
            AudioSource.Play();
        }
        internal void Stop() {
            Assert.Operation.Message( $"Theme {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            AudioSource.Stop();
            AudioSource.clip = null;
        }

    }
}

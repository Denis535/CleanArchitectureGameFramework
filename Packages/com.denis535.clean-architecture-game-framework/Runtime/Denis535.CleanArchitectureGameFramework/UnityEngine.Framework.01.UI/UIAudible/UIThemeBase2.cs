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

        // Constructor
        public UIThemeBase2(IDependencyContainer container, AudioSource audioSource) {
            Container = container;
            AudioSource = audioSource;
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public abstract class UIThemeStateBase2 : UIThemeStateBase {

        // AudioSource
        protected AudioSource AudioSource { get; }
        // IsPlaying
        protected bool IsPlaying {
            get => AudioSource.clip != null;
        }
        protected bool IsCompleted {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( AudioSource.clip != null );
                return !AudioSource.isPlaying && AudioSource.time == AudioSource.clip.length;
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

        // Constructor
        public UIThemeStateBase2(AudioSource audioSource) {
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

    }
}

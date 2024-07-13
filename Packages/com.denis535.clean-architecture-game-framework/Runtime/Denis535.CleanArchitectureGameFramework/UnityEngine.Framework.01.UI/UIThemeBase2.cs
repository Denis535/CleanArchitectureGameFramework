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
        protected internal AudioSource AudioSource { get; }

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

        // Context
        protected new UIThemeBase2 Context => (UIThemeBase2) base.Context;
        // IsPlaying
        protected bool IsPlaying {
            get => Context.AudioSource.clip != null;
        }
        protected bool IsCompleted {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                return !Context.AudioSource.isPlaying && Context.AudioSource.time == Context.AudioSource.clip.length;
            }
        }
        protected bool IsPaused {
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                if (value) {
                    Context.AudioSource.Pause();
                } else {
                    Context.AudioSource.UnPause();
                }
            }
        }
        protected bool Mute {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                return Context.AudioSource.mute;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                Context.AudioSource.mute = value;
            }
        }
        protected float Volume {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                return Context.AudioSource.volume;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                Context.AudioSource.volume = value;
            }
        }
        protected float Pitch {
            get {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                return Context.AudioSource.pitch;
            }
            set {
                Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
                Context.AudioSource.pitch = value;
            }
        }

        // Constructor
        public UIThemeStateBase2(UIThemeBase2 context) : base( context ) {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Play
        protected void Play(AudioClip clip) {
            Assert.Operation.Message( $"Theme {this} must be non-playing" ).Valid( Context.AudioSource.clip == null );
            Context.AudioSource.clip = clip;
            Context.AudioSource.Play();
        }
        protected void Stop() {
            Assert.Operation.Message( $"Theme {this} must be playing" ).Valid( Context.AudioSource.clip != null );
            Context.AudioSource.Stop();
            Context.AudioSource.clip = null;
        }

    }
}

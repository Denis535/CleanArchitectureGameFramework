#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
            Assert.Operation.Message( $"AudioClip {source.clip} must be null" ).Valid( source.clip == null );
            source.clip = clip;
            source.volume = 1;
            source.pitch = 1;
            source.Play();
        }
        protected static void Pause(AudioSource source, bool value) {
            if (value) {
                source.Pause();
            } else {
                source.UnPause();
            }
        }
        protected static void Stop(AudioSource source) {
            Assert.Operation.Message( $"AudioClip must be non-null" ).Valid( source.clip != null );
            source.Stop();
            source.clip = null;
        }
        protected static bool IsCompleted(AudioSource source) {
            return source.clip is not null && Mathf.Approximately( source.time, source.clip.length );
        }

    }
}

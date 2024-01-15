#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class UIViewExtensions {

        // GetAudioSource
        public static AudioSource GetAudioSource(this UIViewBase view) {
            if (view is UIScreenViewBase screenView) {
                return screenView.Screen.AudioSource;
            }
            if (view is UIWidgetViewBase widgetView) {
                return widgetView.Widget.Screen!.AudioSource;
            }
            if (view is UISubViewBase subView) {
                return subView.Widget.Screen!.AudioSource;
            }
            throw Exceptions.Internal.NotSupported( $"View {view} is not supported" );
        }

        // IsClipPlaying
        public static bool IsClipPlaying(this UIViewBase view, AudioClip clip) {
            if (view.GetAudioSource().isPlaying && view.GetAudioSource().clip == clip) {
                return true;
            }
            return false;
        }
        public static bool IsClipPlaying(this UIViewBase view, AudioClip clip, out float time) {
            if (view.GetAudioSource().isPlaying && view.GetAudioSource().clip == clip) {
                time = view.GetAudioSource()!.time;
                return true;
            }
            time = 0;
            return false;
        }

        // PlayClip
        public static void PlayClip(this UIViewBase view, AudioClip clip, float volume = 1) {
            view.GetAudioSource().clip = clip;
            view.GetAudioSource().volume = volume;
            view.GetAudioSource().Play();
        }
        public static void StopClip(this UIViewBase view) {
            view.GetAudioSource().Stop();
        }

    }
}

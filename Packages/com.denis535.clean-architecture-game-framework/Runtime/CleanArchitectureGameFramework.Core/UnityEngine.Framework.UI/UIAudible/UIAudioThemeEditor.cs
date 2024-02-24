#if UNITY_EDITOR
#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor( typeof( UIAudioThemeBase ), true )]
    public class UIAudioThemeEditor : Editor {

        private UIAudioThemeBase Target => (UIAudioThemeBase) target;

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying) {
                LabelField( "Clip", $"{Target.Clip?.name ?? "Null"} ({Target.Clip?.length ?? 0})" );
                LabelField( "IsPlaying", Target.IsPlaying.ToString() );
                LabelField( "IsPaused", Target.IsPaused.ToString() );
                LabelField( "Time", Target.Time.ToString() );
                LabelField( "Volume", Target.Volume.ToString() );
                LabelField( "Mute", Target.Mute.ToString() );
            }
        }
        public override bool RequiresConstantRepaint() {
            return EditorApplication.isPlaying;
        }

        // Helpers
        private static void LabelField(string label, string? text) {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.PrefixLabel( label );
                EditorGUI.SelectableLabel( GUILayoutUtility.GetRect( new GUIContent( text ), GUI.skin.textField ), text, GUI.skin.textField );
            }
        }

    }
}
#endif

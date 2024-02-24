#if UNITY_EDITOR
#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor( typeof( ProgramBase ), true )]
    public class ProgramEditor : Editor {

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            using (new GUILayout.VerticalScope( GUI.skin.box )) {
                if (EditorGUILayout.LinkButton( "denis535.github.io" )) {
                    Application.OpenURL( "https://denis535.github.io" );
                }
                if (EditorGUILayout.LinkButton( "openupm.com" )) {
                    Application.OpenURL( "https://openupm.com/packages/?sort=downloads&q=denis535" );
                }
                if (EditorGUILayout.LinkButton( "assetstore.unity.com" )) {
                    Application.OpenURL( "https://assetstore.unity.com/publishers/90787" );
                }
                if (EditorGUILayout.LinkButton( "itch.io" )) {
                    Application.OpenURL( "https://denis535.itch.io/" );
                }
                EditorGUILayout.SelectableLabel( "If you want to support me, please rate my packages." );
            }
        }

    }
}
#endif

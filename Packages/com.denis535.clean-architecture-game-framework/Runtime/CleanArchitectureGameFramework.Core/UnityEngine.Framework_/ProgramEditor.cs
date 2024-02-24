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
                EditorGUILayout.SelectableLabel( "If you want to support me, please rate my packages:" );
                EditorGUILayout.LinkButton( "https://denis535.github.io" );
                EditorGUILayout.LinkButton( "https://openupm.com/packages/?sort=downloads&q=denis535" );
                EditorGUILayout.LinkButton( "https://assetstore.unity.com/publishers/90787" );
                EditorGUILayout.LinkButton( "https://denis535.itch.io/" );
            }
        }

    }
}

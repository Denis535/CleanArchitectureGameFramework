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

        // Target
        private ProgramBase Target => (ProgramBase) target;

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
        }

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUILayout.Separator();

            using (new GUILayout.VerticalScope( EditorStyles.helpBox )) {
                EditorGUILayout.LabelField( "Overview", EditorStyles.boldLabel );
                EditorGUILayout.LabelField( "This framework helping you to develop your project following the best practices." );
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField( "Links", EditorStyles.boldLabel );
                if (EditorGUILayout.LinkButton( "denis535.github.io" )) {
                    Application.OpenURL( "https://denis535.github.io" );
                }
                if (EditorGUILayout.LinkButton( "Unity Asset Store" )) {
                    Application.OpenURL( "https://assetstore.unity.com/publishers/90787" );
                }
                if (EditorGUILayout.LinkButton( "itch.io" )) {
                    Application.OpenURL( "https://denis535.itch.io/" );
                }
                if (EditorGUILayout.LinkButton( "Unity Package Registry" )) {
                    Application.OpenURL( "https://openupm.com/packages/?sort=downloads&q=denis535" );
                }
                if (EditorGUILayout.LinkButton( "YouTube" )) {
                    Application.OpenURL( "https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg" );
                }
                if (EditorGUILayout.LinkButton( "GitHub" )) {
                    Application.OpenURL( "https://github.com/Denis535/CleanArchitectureGameFramework" );
                }
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField( "If you want to support me", EditorStyles.boldLabel );
                EditorGUILayout.LabelField( "If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos." );
            }
        }
        public override bool RequiresConstantRepaint() {
            return EditorApplication.isPlaying;
        }

    }
}
#endif

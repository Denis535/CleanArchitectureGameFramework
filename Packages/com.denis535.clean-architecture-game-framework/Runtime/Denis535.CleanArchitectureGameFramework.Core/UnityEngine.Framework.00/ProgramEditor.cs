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
        protected ProgramBase Target => (ProgramBase) target;

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (Application.isPlaying && Target.didAwake && Target) {
                Target.OnInspectorGUI();
            } else {
                HelpBox.Draw();
            }
        }
        public override bool RequiresConstantRepaint() {
            return EditorApplication.isPlaying;
        }

    }
    public static class HelpBox {

        public static void Draw() {
            using (new GUILayout.VerticalScope( EditorStyles.helpBox )) {
                {
                    EditorGUILayout.LabelField( "Overview", EditorStyles.boldLabel );
                    EditorGUILayout.LabelField( "This framework helping you to develop your project following the best practices." );
                    EditorGUILayout.Separator();
                }
                {
                    EditorGUILayout.LabelField( "Links", EditorStyles.boldLabel );
                    if (EditorGUILayout.LinkButton( "denis535.github.io" )) Application.OpenURL( "https://denis535.github.io" );
                    if (EditorGUILayout.LinkButton( "github.com (Framework)" )) Application.OpenURL( "https://github.com/Denis535/CleanArchitectureGameFramework" );
                    if (EditorGUILayout.LinkButton( "github.com (Example)" )) Application.OpenURL( "https://github.com/Denis535/CleanGameExample" );
                    EditorGUILayout.Space( 2f );
                    if (EditorGUILayout.LinkButton( "assetstore.unity.com" )) Application.OpenURL( "https://assetstore.unity.com/publishers/90787" );
                    if (EditorGUILayout.LinkButton( "openupm.com" )) Application.OpenURL( "https://openupm.com/packages/?sort=downloads&q=denis535" );
                    if (EditorGUILayout.LinkButton( "nuget.org" )) Application.OpenURL( "https://www.nuget.org/profiles/Denis535" );
                    EditorGUILayout.Space( 2f );
                    if (EditorGUILayout.LinkButton( "itch.io" )) Application.OpenURL( "https://denis535.itch.io/" );
                    if (EditorGUILayout.LinkButton( "youtube.com" )) Application.OpenURL( "https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg" );
                    EditorGUILayout.Separator();
                }
                {
                    EditorGUILayout.LabelField( "If you want to support me", EditorStyles.boldLabel );
                    EditorGUILayout.LabelField( "If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos." );
                }
            }
        }

    }
}
#endif

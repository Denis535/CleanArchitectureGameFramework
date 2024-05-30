#nullable enable
namespace CleanArchitectureGameFramework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class AboutWindow : EditorWindow {

        // Show
        [MenuItem( "Tools/Clean Architecture Game Framework/About Clean Architecture Game Framework", priority = 10000 )]
        public new static void Show() {
            var window = GetWindow<AboutWindow>( true, "About Clean Architecture Game Framework", true );
            window.minSize = window.maxSize = new Vector2( 800, 600 );
        }

        // OnGUI
        public void OnGUI() {
            using (new GUILayout.VerticalScope( EditorStyles.helpBox )) {
                EditorGUILayout.LabelField( "Overview", EditorStyles.boldLabel );
                EditorGUILayout.LabelField( "This framework helping you to develop your project following the best practices." );
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField( "Links", EditorStyles.boldLabel );
                {
                    if (EditorGUILayout.LinkButton( "denis535.github.io" )) Application.OpenURL( "https://denis535.github.io" );
                    if (EditorGUILayout.LinkButton( "github.com" )) Application.OpenURL( "https://github.com/Denis535/CleanArchitectureGameFramework" );
                    EditorGUILayout.Space( 2f );
                    if (EditorGUILayout.LinkButton( "assetstore.unity.com" )) Application.OpenURL( "https://assetstore.unity.com/publishers/90787" );
                    if (EditorGUILayout.LinkButton( "openupm.com" )) Application.OpenURL( "https://openupm.com/packages/?sort=downloads&q=denis535" );
                    if (EditorGUILayout.LinkButton( "nuget.org" )) Application.OpenURL( "https://www.nuget.org/profiles/Denis535" );
                    EditorGUILayout.Space( 2f );
                    if (EditorGUILayout.LinkButton( "youtube.com" )) Application.OpenURL( "https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg" );
                    if (EditorGUILayout.LinkButton( "itch.io" )) Application.OpenURL( "https://denis535.itch.io/" );
                    EditorGUILayout.Separator();
                }

                EditorGUILayout.LabelField( "If you want to support me", EditorStyles.boldLabel );
                EditorGUILayout.LabelField( "If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos." );
            }
        }

    }
}

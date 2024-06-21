#if UNITY_EDITOR
#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Framework.App;
    using UnityEngine.Framework.Entities;
    using UnityEngine.Framework.UI;

    [CustomEditor( typeof( ProgramBase2 ), true )]
    public class ProgramEditor : Editor {

        // Target
        protected ProgramBase2 Target => (ProgramBase2) target;
        // Container
        protected virtual IDependencyContainer Container => Target;
        protected virtual UIThemeBase? Theme => Container.GetDependency<UIThemeBase>();
        protected virtual UIScreenBase? Screen => Container.GetDependency<UIScreenBase>();
        protected virtual UIRouterBase? Router => Container.GetDependency<UIRouterBase>();
        protected virtual ApplicationBase? Application => Container.GetDependency<ApplicationBase>();
        protected virtual GameBase? Game => Container.GetDependency<GameBase>();

        // Awake
        protected virtual void Awake() {
        }
        protected virtual void OnDestroy() {
        }

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (UnityEngine.Application.isPlaying && Target.didAwake) {
                DrawGUI( Theme );
                DrawGUI( Screen );
                DrawGUI( Router );
                DrawGUI( Application );
                DrawGUI( Game );
            } else {
                DrawHelp();
            }
        }
        public override bool RequiresConstantRepaint() {
            return EditorApplication.isPlaying;
        }

        // DrawGUI
        protected virtual void DrawGUI(UIThemeBase? theme) {
            LabelField( "Theme", theme?.ToString() ?? "Null" );
        }
        protected virtual void DrawGUI(UIScreenBase? screen) {
            LabelField( "Screen", screen?.ToString() ?? "Null" );
            LabelField( "Widget", GetDisplayString( screen?.Widget ) ?? "Null" );
            LabelField( "View", GetDisplayString( screen?.Widget?.View ) ?? "Null" );
        }
        protected virtual void DrawGUI(UIRouterBase? router) {
            LabelField( "Router", router?.ToString() ?? "Null" );
        }
        protected virtual void DrawGUI(ApplicationBase? application) {
            LabelField( "Application", application?.ToString() ?? "Null" );
        }
        protected virtual void DrawGUI(GameBase? game) {
            LabelField( "Game", game?.ToString() ?? "Null" );
        }
        // DrawHelp
        protected virtual void DrawHelp() {
            using (new GUILayout.VerticalScope( EditorStyles.helpBox )) {
                {
                    EditorGUILayout.LabelField( "Overview", EditorStyles.boldLabel );
                    EditorGUILayout.LabelField( "This framework helping you to develop your project following the best practices." );
                    EditorGUILayout.Separator();
                }
                {
                    EditorGUILayout.LabelField( "Links", EditorStyles.boldLabel );
                    if (EditorGUILayout.LinkButton( "denis535.github.io" )) UnityEngine.Application.OpenURL( "https://denis535.github.io" );
                    if (EditorGUILayout.LinkButton( "github.com" )) UnityEngine.Application.OpenURL( "https://github.com/Denis535/CleanArchitectureGameFramework" );
                    EditorGUILayout.Space( 2f );
                    if (EditorGUILayout.LinkButton( "assetstore.unity.com" )) UnityEngine.Application.OpenURL( "https://assetstore.unity.com/publishers/90787" );
                    if (EditorGUILayout.LinkButton( "openupm.com" )) UnityEngine.Application.OpenURL( "https://openupm.com/packages/?sort=downloads&q=denis535" );
                    if (EditorGUILayout.LinkButton( "nuget.org" )) UnityEngine.Application.OpenURL( "https://www.nuget.org/profiles/Denis535" );
                    EditorGUILayout.Space( 2f );
                    if (EditorGUILayout.LinkButton( "youtube.com" )) UnityEngine.Application.OpenURL( "https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg" );
                    if (EditorGUILayout.LinkButton( "itch.io" )) UnityEngine.Application.OpenURL( "https://denis535.itch.io/" );
                    EditorGUILayout.Separator();
                }
                {
                    EditorGUILayout.LabelField( "If you want to support me", EditorStyles.boldLabel );
                    EditorGUILayout.LabelField( "If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos." );
                }
            }
        }

        // Helpers
        protected static void LabelField(string label, string? text) {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.PrefixLabel( label );
                EditorGUI.SelectableLabel( GUILayoutUtility.GetRect( new GUIContent( text ), GUI.skin.textField ), text, GUI.skin.textField );
            }
        }
        // Helpers
        protected static string? GetDisplayString(UIWidgetBase? widget) {
            if (widget == null) return null;
            var builder = new StringBuilder();
            builder.AppendHierarchy( widget, i => i.GetType().Name, i => i.Children );
            return builder.ToString();
        }
        protected static string? GetDisplayString(UIViewBase? view) {
            if (view == null) return null;
            var builder = new StringBuilder();
            builder.AppendHierarchy( view, i => i.GetType().Name, i => i.Children );
            return builder.ToString();
        }

    }
}
#endif

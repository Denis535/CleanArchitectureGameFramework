#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.Framework.UI;
    using UnityEngine.Framework.App;
    using UnityEngine.Framework.Domain;
    using UnityEngine.UIElements;

    [DefaultExecutionOrder( 10 )]
    public abstract partial class ProgramBase : MonoBehaviour {

        // Awake
        protected virtual void Awake() {
            Application.wantsToQuit += OnQuit;
        }
        protected virtual void OnDestroy() {
        }

        // Start
        protected virtual void Start() {
        }
        protected virtual void FixedUpdate() {
        }
        protected virtual void Update() {
        }
        protected virtual void LateUpdate() {
        }

        // OnQuit
        protected virtual bool OnQuit() {
            return true;
        }

    }
#if UNITY_EDITOR
    public abstract partial class ProgramBase {

        // OnInspectorGUI
        protected internal virtual void OnInspectorGUI() {
        }
        protected virtual void OnInspectorGUI(UIThemeBase theme, UIScreenBase screen, UIRouterBase router, ApplicationBase application, GameBase? game) {
            LabelField( "Theme", theme.ToString() );
            LabelField( "PlayList", theme.PlayList?.Pipe( GetDisplayString ) ?? "Null" );
            GUILayout.Space( 2 );
            LabelField( "Screen", screen.ToString() );
            LabelField( "Widget", screen.Widget?.Pipe( GetDisplayString ) ?? "Null" );
            LabelField( "View", screen.Widget?.View?.Pipe( GetDisplayString ) ?? "Null" );
            GUILayout.Space( 2 );
            LabelField( "Router", router.ToString() );
            LabelField( "Application", application.ToString() );
            LabelField( "Game", game?.ToString() ?? "Null" );
        }

        // Helpers
        protected static void LabelField(string label, string? text) {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.PrefixLabel( label );
                EditorGUI.SelectableLabel( GUILayoutUtility.GetRect( new GUIContent( text ), GUI.skin.textField ), text, GUI.skin.textField );
            }
        }
        // Helpers
        protected static string? GetDisplayString(UIPlayListBase playList) {
            return playList.ToString();
        }
        protected static string? GetDisplayString(UIWidgetBase widget) {
            var builder = new StringBuilder();
            builder.AppendHierarchy( widget, i => i.ToString(), i => i.Children );
            return builder.ToString();
        }
        protected static string? GetDisplayString(UIViewBase view) {
            var builder = new StringBuilder();
            builder.AppendHierarchy( (VisualElement) view, i => $"{i.GetType().FullName} ({i.name})", i => i.Children() );
            return builder.ToString();
        }

    }
#endif
}

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

    [DefaultExecutionOrder( 1000 )]
    public abstract class ProgramBase : MonoBehaviour {

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

#if UNITY_EDITOR
        // OnInspectorGUI
        protected internal virtual void OnInspectorGUI() {
        }
        protected virtual void OnInspectorGUI(UIThemeBase theme) {
            LabelField( "Theme", theme.ToString() );
        }
        protected virtual void OnInspectorGUI(UIScreenBase screen) {
            LabelField( "Screen", screen.ToString() );
            LabelField( "Widget", screen?.Widget.Chain( GetDisplayString ) ?? "Null" );
            LabelField( "View", screen?.Widget?.View.Chain( GetDisplayString ) ?? "Null" );
        }
        protected virtual void OnInspectorGUI(UIRouterBase router) {
            LabelField( "Router", router.ToString() );
        }
        protected virtual void OnInspectorGUI(ApplicationBase application) {
            LabelField( "Application", application.ToString() );
        }
        protected virtual void OnInspectorGUI(GameBase? game) {
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
#endif

    }
}

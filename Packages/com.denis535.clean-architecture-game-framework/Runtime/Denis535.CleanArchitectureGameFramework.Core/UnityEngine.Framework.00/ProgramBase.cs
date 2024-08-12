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
    using UnityEngine.Framework.Entities;
    using UnityEngine.UIElements;

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
        protected virtual void OnInspectorGUI(UIThemeBase theme) {
            LabelField( "Theme", theme.ToString() );
        }
        protected virtual void OnInspectorGUI(UIScreenBase screen, UIWidgetBase? widget, UIViewBase? view) {
            LabelField( "Screen", screen.ToString() );
            LabelField( "Widget", widget?.Chain( GetDisplayString ) ?? "Null" );
            LabelField( "View", view?.Chain( GetDisplayString ) ?? "Null" );
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

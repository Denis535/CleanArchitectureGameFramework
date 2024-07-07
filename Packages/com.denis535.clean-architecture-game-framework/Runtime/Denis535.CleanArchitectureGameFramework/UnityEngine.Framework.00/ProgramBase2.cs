#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Framework.UI;
    using UnityEngine.Framework.App;
    using UnityEngine.Framework.Entities;
    using UnityEngine.UIElements;

    public abstract class ProgramBase2 : ProgramBase, IDependencyContainer {

        // IDependencyContainer
        Option<object?> IDependencyContainer.GetValue(Type type, object? argument) {
            return GetValue( type, argument );
        }
        protected abstract Option<object?> GetValue(Type type, object? argument);

    }
    public abstract class ProgramBase2<TTheme, TScreen, TRouter, TApplication, TGame> : ProgramBase2
        where TTheme : notnull, UIThemeBase
        where TScreen : notnull, UIScreenBase
        where TRouter : notnull, UIRouterBase
        where TApplication : notnull, ApplicationBase
        where TGame : notnull, GameBase {

        // Framework
        protected abstract TTheme Theme { get; set; }
        protected abstract TScreen Screen { get; set; }
        protected abstract TRouter Router { get; set; }
        protected abstract TApplication Application { get; set; }
        protected abstract TGame? Game { get; }

        // Awake
        protected override void Awake() {
            base.Awake();
        }
        protected override void OnDestroy() {
            base.OnDestroy();
        }

#if UNITY_EDITOR
        // OnInspectorGUI
        protected internal override void OnInspectorGUI() {
            OnInspectorGUI( Theme, Theme.State );
            OnInspectorGUI( Screen, Screen.Widget, Screen.Widget?.View, (Screen.Widget?.View as UIViewBase2)?.VisualElement );
            OnInspectorGUI( Router );
            OnInspectorGUI( Application );
            OnInspectorGUI( Game );
        }
        protected virtual void OnInspectorGUI(UIThemeBase theme, UIThemeStateBase? state) {
            LabelField( "Theme", theme.ToString() );
            LabelField( "Theme State", state?.ToString() ?? "Null" );
        }
        protected virtual void OnInspectorGUI(UIScreenBase screen, UIWidgetBase? widget, UIViewBase? view, VisualElement? visualElement) {
            LabelField( "Screen", screen.ToString() );
            LabelField( "Widget", widget?.Chain( GetDisplayString ) ?? "Null" );
            LabelField( "View", view?.Chain( GetDisplayString ) ?? "Null" );
            LabelField( "VisualElement", visualElement?.Chain( GetDisplayString ) ?? "Null" );
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
            builder.AppendHierarchy( view, i => i.ToString(), i => i.Children );
            return builder.ToString();
        }
        protected static string? GetDisplayString(VisualElement visualElement) {
            var builder = new StringBuilder();
            builder.AppendHierarchy( visualElement, i => $"{i.GetType().FullName} ({i.name})", i => i.Children() );
            return builder.ToString();
        }
#endif

    }
}

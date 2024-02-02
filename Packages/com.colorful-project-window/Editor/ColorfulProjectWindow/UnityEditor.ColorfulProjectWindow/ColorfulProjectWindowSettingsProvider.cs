#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ColorfulProjectWindowSettingsProvider : SettingsProvider {

        private ColorfulProjectWindowSettings Settings => ColorfulProjectWindowSettings.Instance;

        // Constructor
        public ColorfulProjectWindowSettingsProvider() : base( "Preferences/Colorful Project Window", SettingsScope.User, new[] { "Colorful Project Window" } ) {
        }

        // OnActivate
        public override void OnActivate(string searchContext, VisualElement rootElement) {
        }
        public override void OnDeactivate() {
        }

        // OnGUI
        public override void OnTitleBarGUI() {
        }
        public override void OnGUI(string searchContext) {
            using (var scope = new EditorGUI.ChangeCheckScope()) {
                using (new EditorGUILayout.VerticalScope( GUI.skin.box )) {
                    Settings.ModuleColor = EditorGUILayout.ColorField( "Module Color", Settings.ModuleColor );
                    Settings.AssetsColor = EditorGUILayout.ColorField( "Assets Color", Settings.AssetsColor );
                    Settings.ResourcesColor = EditorGUILayout.ColorField( "Resources Color", Settings.ResourcesColor );
                    Settings.SourcesColor = EditorGUILayout.ColorField( "Sources Color", Settings.SourcesColor );
                    if (GUILayout.Button( "Reset", GUILayout.ExpandWidth( false ) )) {
                        Settings.Reset();
                    }
                }
                if (scope.changed) {
                    var window = (EditorWindow?) Resources.FindObjectsOfTypeAll( Type.GetType( "UnityEditor.ProjectBrowser, UnityEditor.CoreModule" ) ).FirstOrDefault();
                    window?.Repaint();
                    Settings.Save();
                }
            }
        }
        public override void OnFooterBarGUI() {
        }

        // OnUpdate
        public override void OnInspectorUpdate() {
        }

        // GetSettingsProvider
        [SettingsProvider]
        public static SettingsProvider? GetSettingsProvider() {
            return new ColorfulProjectWindowSettingsProvider();
        }

    }
}

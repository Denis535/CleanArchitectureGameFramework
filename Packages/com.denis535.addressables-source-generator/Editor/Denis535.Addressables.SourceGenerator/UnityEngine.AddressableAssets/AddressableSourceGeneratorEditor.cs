#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor( typeof( AddressableSourceGenerator ) )]
    public class AddressableSourceGeneratorEditor : Editor {

        // Target
        private AddressableSourceGenerator Target => (AddressableSourceGenerator) target;

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
        }

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            LabelField( "Resources Class Namespace", Target.ResourcesClassNamespace );
            LabelField( "Resources Class Name", Target.ResourcesClassName );
            EditorGUILayout.Separator();
            LabelField( "Labels Class Namespace", Target.LabelsClassNamespace );
            LabelField( "Labels Class Name", Target.LabelsClassName );
            EditorGUILayout.Separator();
            if (GUILayout.Button( "Generate" )) {
                Target.Generate();
            }
        }

        // Heleprs
        private static void LabelField(string label, string? text) {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.PrefixLabel( label );
                EditorGUI.SelectableLabel( GUILayoutUtility.GetRect( new GUIContent( text ), GUI.skin.textField ), text, GUI.skin.textField );
            }
        }

    }
}

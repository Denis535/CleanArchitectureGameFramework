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
            LabelField( "Namespace", Target.Namespace );
            LabelField( "Resources Class Name", Target.ResourcesClassName );
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
                EditorGUI.SelectableLabel( GUILayoutUtility.GetRect( new GUIContent( text ), UnityEngine.GUI.skin.textField ), text, UnityEngine.GUI.skin.textField );
            }
        }

    }
}

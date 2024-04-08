#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CustomEditor( typeof( AddressablesSourceGenerator ) )]
    public class AddressablesSourceGeneratorEditor : Editor {

        public AddressablesSourceGenerator Target => (AddressablesSourceGenerator) target;

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            LabelField( "Directory", Target.Directory );
            EditorGUILayout.Separator();

            LabelField( "Namespace", Target.Namespace );
            LabelField( "Resources Class", Target.ResourcesClass );
            LabelField( "Labels Class", Target.LabelsClass );
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

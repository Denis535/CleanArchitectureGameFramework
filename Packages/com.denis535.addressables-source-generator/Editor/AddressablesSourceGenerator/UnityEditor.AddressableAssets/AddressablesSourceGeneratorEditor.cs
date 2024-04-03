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
            if (GUILayout.Button( "Generate" )) {
                Target.GenerateResourcesSource( "R.cs", "UnityEngine.AddressableAssets", "R" );
                Target.GenerateLabelsSource( "L.cs", "UnityEngine.AddressableAssets", "L" );
            }
        }

    }
}

#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CustomEditor( typeof( AddressablesSourceGenerator ) )]
    public class AddressablesSourceGeneratorEditor : Editor {

        public AddressablesSourceGenerator Target => (AddressablesSourceGenerator) target;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button( "Generate" )) {
                Target.Generate();
            }
        }

    }
}

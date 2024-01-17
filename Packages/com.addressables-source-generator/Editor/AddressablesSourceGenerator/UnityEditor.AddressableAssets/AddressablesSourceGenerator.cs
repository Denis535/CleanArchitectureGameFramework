#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    [CreateAssetMenu( fileName = "AddressablesSourceGenerator", menuName = "Addressables/AddressablesSourceGenerator" )]
    public class AddressablesSourceGenerator : ScriptableObject {

        public virtual void Generate() {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var path = AssetDatabase.GetAssetPath( this );
            var dir = Path.GetDirectoryName( path );
            new ResourcesSourceGenerator().Generate( settings, Path.Combine( dir, "R.cs" ), "UnityEngine.AddressableAssets", "R" );
            new LabelsSourceGenerator().Generate( settings, Path.Combine( dir, "L.cs" ), "UnityEngine.AddressableAssets", "L" );
        }

    }
}

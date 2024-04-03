#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    [CreateAssetMenu( fileName = "AddressablesSourceGenerator", menuName = "Addressables/AddressablesSourceGenerator" )]
    public class AddressablesSourceGenerator : ScriptableObject {

        // Generate
        public void GenerateResourcesSource(string path, string @namespace, string name) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            path = Path.Combine( Path.GetDirectoryName( AssetDatabase.GetAssetPath( this ) ), path );
            new ResourcesSourceGenerator().Generate( settings, path, @namespace, name );
        }
        public void GenerateLabelsSource(string path, string @namespace, string name) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            path = Path.Combine( Path.GetDirectoryName( AssetDatabase.GetAssetPath( this ) ), path );
            new LabelsSourceGenerator().Generate( settings, path, @namespace, name );
        }

    }
}

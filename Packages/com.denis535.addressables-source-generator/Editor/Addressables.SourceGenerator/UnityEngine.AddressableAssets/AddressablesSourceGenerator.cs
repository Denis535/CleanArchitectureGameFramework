#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEngine;

    [CreateAssetMenu( fileName = "AddressablesSourceGenerator", menuName = "Addressables/AddressablesSourceGenerator" )]
    public class AddressablesSourceGenerator : ScriptableObject {

        public string Directory => Path.GetDirectoryName( AssetDatabase.GetAssetPath( this ) );
        public string Namespace => new DirectoryInfo( Directory ).Name;
        public string ResourcesClassName => "R";
        public string LabelsClassName => "L";

        // Generate
        public void Generate() {
            GenerateResourcesSource( Path.Combine( Directory, ResourcesClassName + ".cs" ), Namespace, ResourcesClassName );
            GenerateLabelsSource( Path.Combine( Directory, LabelsClassName + ".cs" ), Namespace, LabelsClassName );
        }
        private static void GenerateResourcesSource(string path, string @namespace, string name) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            new ResourcesSourceGenerator().Generate( settings, path, @namespace, name );
        }
        private static void GenerateLabelsSource(string path, string @namespace, string name) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            new LabelsSourceGenerator().Generate( settings, path, @namespace, name );
        }

    }
}

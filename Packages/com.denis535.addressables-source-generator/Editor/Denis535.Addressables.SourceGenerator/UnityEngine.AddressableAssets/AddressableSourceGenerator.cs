#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEngine;

    [CreateAssetMenu( fileName = "AddressableSourceGenerator", menuName = "Addressables/AddressableSourceGenerator" )]
    public class AddressableSourceGenerator : ScriptableObject {

        // Directory
        public string Directory => Path.GetDirectoryName( AssetDatabase.GetAssetPath( this ) );
        // Namespace
        public string Namespace => new DirectoryInfo( Directory ).Name;
        // ClassName
        public string ResourcesClassName => "R";
        public string LabelsClassName => "L";

        // Generate
        public void Generate() {
            GenerateResourcesSource( Path.Combine( Directory, ResourcesClassName + ".cs" ), Namespace, ResourcesClassName );
            GenerateLabelsSource( Path.Combine( Directory, LabelsClassName + ".cs" ), Namespace, LabelsClassName );
        }
        private static void GenerateResourcesSource(string path, string @namespace, string name) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            new AddressableResourcesSourceGenerator().Generate( settings, path, @namespace, name );
        }
        private static void GenerateLabelsSource(string path, string @namespace, string name) {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            new AddressableLabelsSourceGenerator().Generate( settings, path, @namespace, name );
        }

    }
}

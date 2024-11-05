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
        private string Directory => Path.GetDirectoryName( AssetDatabase.GetAssetPath( this ) );
        // Path
        public string ResourcesPath => Path.Combine( Directory, ResourcesClassName + ".cs" );
        public string LabelsPath => Path.Combine( Directory, LabelsClassName + ".cs" );
        // Namespace
        public string ResourcesClassNamespace => new DirectoryInfo( Directory ).Name;
        public string LabelsClassNamespace => new DirectoryInfo( Directory ).Name;
        // Name
        public string ResourcesClassName => "R";
        public string LabelsClassName => "L";

        // Generate
        public void Generate() {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            new ResourcesSourceGenerator().Generate( ResourcesPath, ResourcesClassNamespace, ResourcesClassName, settings );
            new LabelsSourceGenerator().Generate( LabelsPath, LabelsClassNamespace, LabelsClassName, settings );
        }

    }
}

#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    public class AddressableLabelsSourceGenerator {

        // Generate
        public virtual void Generate(AddressableAssetSettings settings, string path, string @namespace, string name) {
            var treeList = GetTreeList( settings.GetLabels().Where( IsSupported ) );
            Generate( path, @namespace, name, treeList );
        }
        public virtual void Generate(string path, string @namespace, string name, KeyValueTreeList<string> treeList) {
            var builder = new StringBuilder();
            builder.AppendCompilationUnit( @namespace, name, treeList );
            WriteText( path, builder.ToString() );
        }

        // IsSupported
        protected virtual bool IsSupported(string label) {
            return true;
        }

        // Helpers
        private static KeyValueTreeList<string> GetTreeList(IEnumerable<string> labels) {
            var treeList = new KeyValueTreeList<string>();
            foreach (var label in labels) {
                var path = GetPath( label );
                treeList.AddValue( path.SkipLast( 1 ), path.Last(), label );
            }
            return treeList;
        }
        private static string[] GetPath(string label) {
            return label.Split( '/', '\\', '.' );
        }
        // Helpers
        private static void WriteText(string path, string text) {
            if (!File.Exists( path ) || File.ReadAllText( path ) != text) {
                File.WriteAllText( path, text );
                AssetDatabase.ImportAsset( path, ImportAssetOptions.Default );
            }
        }

    }
}

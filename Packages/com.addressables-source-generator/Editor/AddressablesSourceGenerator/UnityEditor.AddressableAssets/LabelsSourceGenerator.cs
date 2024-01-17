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

    public class LabelsSourceGenerator {

        // Generate
        public void Generate(AddressableAssetSettings settings, string path, string @namespace, string name) {
            var builder = new StringBuilder();
            var treeList = LabelsSourceGeneratorHelper.GetTreeList( settings.GetLabels().Where( IsSupported ) );
            AppendCompilationUnit( builder, @namespace, name, treeList );
            WriteText( path, builder.ToString() );
        }

        // AppendCompilationUnit
        private void AppendCompilationUnit(StringBuilder builder, string @namespace, string name, KeyValueTreeList<string> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                AppendClass( builder, 1, name, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private void AppendClass(StringBuilder builder, int indent, string name, KeyValueTreeList<string>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{name} {{" );
            foreach (var item in items) {
                if (item is KeyValueTreeList<string>.ValueItem value) {
                    var key = Escape( value.Key );
                    if (key == name) key = $"{key}_";
                    var value_ = value.Value;
                    AppendConst( builder, indent + 1, key, value_ );
                } else
                if (item is KeyValueTreeList<string>.ListItem list) {
                    var key = Escape( list.Key );
                    if (key == name) key = $"{key}_";
                    var items_ = list.Items.ToArray();
                    AppendClass( builder, indent + 1, key, items_ );
                }
            }
            builder.AppendIndent( indent ).AppendLine( "}" );
        }
        private void AppendConst(StringBuilder builder, int indent, string name, string value) {
            builder.AppendIndent( indent ).AppendLine( $"public const string @{name} = \"{value}\";" );
        }

        // IsSupported
        public virtual bool IsSupported(string label) {
            return true;
        }

        // Helpers
        private static string Escape(string value) {
            var chars = value.ToCharArray();
            for (var i = 0; i < chars.Length; i++) {
                if (!char.IsLetterOrDigit( chars[ i ] )) chars[ i ] = '_';
            }
            return new string( chars );
        }
        private static void WriteText(string path, string text) {
            if (!File.Exists( path ) || File.ReadAllText( path ) != text) {
                File.WriteAllText( path, text );
                AssetDatabase.ImportAsset( path, ImportAssetOptions.Default );
            }
        }

    }
}

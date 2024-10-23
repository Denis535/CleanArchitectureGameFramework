#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    internal static class AddressableLabelsSourceGeneratorHelper {

        // AppendCompilationUnit
        public static void AppendCompilationUnit(this StringBuilder builder, string @namespace, string @class, KeyValueTreeList<string> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                builder.AppendClass( 1, @class, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private static void AppendClass(this StringBuilder builder, int indent, string name, KeyValueTreeList<string>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{GetClassIdentifier( name )} {{" );
            foreach (var item in items) {
                if (item is KeyValueTreeList<string>.ValueItem value) {
                    builder.AppendConst( indent + 1, value.Key, value.Value );
                } else
                if (item is KeyValueTreeList<string>.ListItem list) {
                    builder.AppendClass( indent + 1, list.Key, list.Items.ToArray() );
                }
            }
            builder.AppendIndent( indent ).AppendLine( "}" );
        }
        private static void AppendConst(this StringBuilder builder, int indent, string name, string value) {
            builder.AppendIndent( indent ).AppendLine( $"public const string @{GetConstIdentifier( name )} = \"{value}\";" );
        }

        // Helpers
        private static string GetClassIdentifier(string key) {
            return AddressableResourcesSourceGeneratorHelper.GetClassIdentifier( key );
        }
        private static string GetConstIdentifier(string key) {
            return AddressableResourcesSourceGeneratorHelper.GetConstIdentifier( key );
        }

    }
}

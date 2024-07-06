#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    internal static class AddressableSourceGeneratorHelper {

        // AppendCompilationUnit
        public static void AppendCompilationUnit(this StringBuilder builder, string @namespace, string name, KeyValueTreeList<AddressableAssetEntry> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                builder.AppendClass( 1, name, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private static void AppendClass(this StringBuilder builder, int indent, string name, KeyValueTreeList<AddressableAssetEntry>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{GetClassName( name )} {{" );
            foreach (var item in Sort( items )) {
                if (item is KeyValueTreeList<AddressableAssetEntry>.ValueItem value) {
                    builder.AppendConst( indent + 1, value.Key, value.Value );
                } else
                if (item is KeyValueTreeList<AddressableAssetEntry>.ListItem list) {
                    builder.AppendClass( indent + 1, list.Key, list.Items.ToArray() );
                }
            }
            builder.AppendIndent( indent ).AppendLine( "}" );
        }
        private static void AppendConst(this StringBuilder builder, int indent, string name, AddressableAssetEntry value) {
            if (value.IsFolder) {
                throw new NotSupportedException( $"Entry {value} is not supported" );
            }
            builder.AppendIndent( indent ).AppendLine( $"public const string @{GetConstName( name )} = \"{value.address}\";" );
        }

        // AppendCompilationUnit
        public static void AppendCompilationUnit(this StringBuilder builder, string @namespace, string name, KeyValueTreeList<string> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                builder.AppendClass( 1, name, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private static void AppendClass(this StringBuilder builder, int indent, string name, KeyValueTreeList<string>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{GetClassName( name )} {{" );
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
            builder.AppendIndent( indent ).AppendLine( $"public const string @{GetConstName( name )} = \"{value}\";" );
        }

        // Helpers
        private static IEnumerable<KeyValueTreeList<AddressableAssetEntry>.Item> Sort(IEnumerable<KeyValueTreeList<AddressableAssetEntry>.Item> items) {
            return items
                .OrderByDescending( i => i is KeyValueTreeList<AddressableAssetEntry>.ValueItem )

                .ThenByDescending( i => i.Key.Equals( "UnityEngine" ) )
                .ThenByDescending( i => i.Key.Equals( "UnityEditor" ) )

                .ThenByDescending( i => i.Key.Equals( "EditorSceneList" ) )
                .ThenByDescending( i => i.Key.Equals( "Resources" ) )

                .ThenByDescending( i => i.Key.Equals( "Project" ) )
                .ThenByDescending( i => i.Key.Equals( "Presentation" ) )
                .ThenByDescending( i => i.Key.Equals( "UI" ) )
                .ThenByDescending( i => i.Key.Equals( "GUI" ) )
                .ThenByDescending( i => i.Key.Equals( "Application" ) )
                .ThenByDescending( i => i.Key.Equals( "App" ) )
                .ThenByDescending( i => i.Key.Equals( "Domain" ) )
                .ThenByDescending( i => i.Key.Equals( "Entities" ) )
                .ThenByDescending( i => i.Key.Equals( "Core" ) )
                .ThenByDescending( i => i.Key.Equals( "Internal" ) )

                .ThenByDescending( i => i.Key.Equals( "Launcher" ) )
                .ThenByDescending( i => i.Key.Equals( "Startup" ) )
                .ThenByDescending( i => i.Key.Equals( "Main" ) )
                .ThenByDescending( i => i.Key.Equals( "Game" ) )
                .ThenByDescending( i => i.Key.Equals( "World" ) )
                .ThenByDescending( i => i.Key.Equals( "Level" ) )

                .ThenByDescending( i => i.Key.Equals( "LauncherScene" ) )
                .ThenByDescending( i => i.Key.Equals( "StartupScene" ) )
                .ThenByDescending( i => i.Key.Equals( "MainScene" ) )
                .ThenByDescending( i => i.Key.Equals( "GameScene" ) )
                .ThenByDescending( i => i.Key.Equals( "WorldScene" ) )
                .ThenByDescending( i => i.Key.Equals( "LevelScene" ) )

                .ThenByDescending( i => i.Key.Equals( "MainScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "GameScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "DebugScreen" ) )

                .ThenByDescending( i => i.Key.Equals( "Characters" ) )
                .ThenByDescending( i => i.Key.Equals( "Things" ) )
                .ThenByDescending( i => i.Key.Equals( "Worlds" ) )
                .ThenByDescending( i => i.Key.Equals( "Levels" ) )

                .ThenByDescending( i => i.Key.Equals( "Common" ) )
                .ThenBy( i => i.Key );
        }
        private static string GetClassName(string key) {
            key = key.Replace( ' ', '_' ).Replace( '-', '_' ).Replace( '@', '_' );
            key = key.TrimStart( ' ', '-', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' );
            key = key.TrimEnd( ' ', '-', '_' );
            return key;
        }
        private static string GetConstName(string key) {
            key = key.Replace( ' ', '_' ).Replace( '-', '_' ).Replace( '@', '_' );
            key = key.TrimStart( ' ', '-', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' );
            key = key.TrimEnd( ' ', '-', '_' );
            return "Value_" + key;
        }

    }
}

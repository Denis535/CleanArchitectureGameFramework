#nullable enable
namespace UnityEditor.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    internal static class SourceGeneratorHelper {

        // AppendCompilationUnit
        public static void AppendCompilationUnit(this StringBuilder builder, string @namespace, string @class, KeyValueTreeList<string> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                builder.AppendClass( 1, @class, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private static void AppendClass(this StringBuilder builder, int indent, string @class, KeyValueTreeList<string>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{@class} {{" );
            foreach (var item in items) {
                if (item is KeyValueTreeList<string>.ValueItem value) {
                    builder.AppendConst( indent + 1, Escape( value.Key, @class ), value.Value );
                } else
                if (item is KeyValueTreeList<string>.ListItem list) {
                    builder.AppendClass( indent + 1, Escape( list.Key, @class ), list.Items.ToArray() );
                }
            }
            builder.AppendIndent( indent ).AppendLine( "}" );
        }
        private static void AppendConst(this StringBuilder builder, int indent, string @const, string value) {
            builder.AppendIndent( indent ).AppendLine( $"public const string @{@const} = \"{value}\";" );
        }

        // AppendCompilationUnit
        public static void AppendCompilationUnit(this StringBuilder builder, string @namespace, string @class, KeyValueTreeList<AddressableAssetEntry> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                builder.AppendClass( 1, @class, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private static void AppendClass(this StringBuilder builder, int indent, string @class, KeyValueTreeList<AddressableAssetEntry>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{@class} {{" );
            foreach (var item in Sort( items )) {
                if (item is KeyValueTreeList<AddressableAssetEntry>.ValueItem value) {
                    builder.AppendConst( indent + 1, Escape( value.Key, @class ), value.Value );
                } else
                if (item is KeyValueTreeList<AddressableAssetEntry>.ListItem list) {
                    builder.AppendClass( indent + 1, Escape( list.Key, @class ), list.Items.ToArray() );
                }
            }
            builder.AppendIndent( indent ).AppendLine( "}" );
        }
        private static void AppendConst(this StringBuilder builder, int indent, string @const, AddressableAssetEntry value) {
            if (value.IsFolder) {
                throw new NotSupportedException( $"Entry {value} is not supported" );
            }
            builder.AppendIndent( indent ).AppendLine( $"public const string @{@const} = \"{value.address}\";" );
        }

        // Helpers
        private static IEnumerable<KeyValueTreeList<AddressableAssetEntry>.Item> Sort(IEnumerable<KeyValueTreeList<AddressableAssetEntry>.Item> items) {
            return items
                .OrderByDescending( i => i.Key.Equals( "UnityEngine" ) )
                .ThenByDescending( i => i.Key.Equals( "UnityEditor" ) )

                .ThenByDescending( i => i.Key.Equals( "EditorSceneList" ) )
                .ThenByDescending( i => i.Key.Equals( "Resources" ) )

                .ThenByDescending( i => i.Key.Equals( "Project" ) )
                .ThenByDescending( i => i.Key.Equals( "Presentation" ) )
                .ThenByDescending( i => i.Key.Equals( "UI" ) )
                .ThenByDescending( i => i.Key.Equals( "GUI" ) )
                .ThenByDescending( i => i.Key.Equals( "App" ) )
                .ThenByDescending( i => i.Key.Equals( "Application" ) )
                .ThenByDescending( i => i.Key.Equals( "Domain" ) )
                .ThenByDescending( i => i.Key.Equals( "Entities" ) )
                .ThenByDescending( i => i.Key.Equals( "Worlds" ) )
                .ThenByDescending( i => i.Key.Equals( "Common" ) )
                .ThenByDescending( i => i.Key.Equals( "Core" ) )
                .ThenByDescending( i => i.Key.Equals( "Internal" ) )

                .ThenByDescending( i => i.Key.Equals( "Launcher" ) )
                .ThenByDescending( i => i.Key.Equals( "LauncherScene" ) )
                .ThenByDescending( i => i.Key.Equals( "Startup" ) )
                .ThenByDescending( i => i.Key.Equals( "StartupScene" ) )
                .ThenByDescending( i => i.Key.Equals( "Program" ) )
                .ThenByDescending( i => i.Key.Equals( "ProgramScene" ) )
                .ThenByDescending( i => i.Key.Equals( "MainScene" ) )
                .ThenByDescending( i => i.Key.Equals( "GameScene" ) )
                .ThenByDescending( i => i.Key.Equals( "World" ) )
                .ThenByDescending( i => i.Key.Equals( "WorldScene" ) )
                .ThenByDescending( i => i.Key.Equals( "Level" ) )
                .ThenByDescending( i => i.Key.Equals( "LevelScene" ) )

                .ThenByDescending( i => i.Key.Equals( "MainScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "GameScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "DebugScreen" ) )

                .ThenBy( i => i is KeyValueTreeList<AddressableAssetEntry>.ValueItem )
                .ThenBy( i => i.Key );
        }
        private static string Escape(string name, string? outer) {
            name = name.TrimStart( ' ', '-', '_' ).TrimStart( '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' );
            name = name.TrimEnd( ' ', '-', '_' );
            name = name.Replace( ' ', '_' ).Replace( '-', '_' ).Replace( '@', '_' );
            return name == outer ? (name + "_") : name;
        }

    }
}

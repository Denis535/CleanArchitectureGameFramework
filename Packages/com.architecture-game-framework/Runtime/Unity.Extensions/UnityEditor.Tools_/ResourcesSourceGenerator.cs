#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.Tools_ {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    public class ResourcesSourceGenerator {

        // Generate
        public void Generate(string path, string @namespace, string name, AddressableAssetSettings settings) {
            var builder = new StringBuilder();
            var treeList = ResourcesSourceGeneratorHelper.GetTreeList( settings.GetEntries().Where( IsSupported ) );
            AppendCompilationUnit( builder, @namespace, name, treeList );
            WriteText( path, builder.ToString() );
        }

        // AppendCompilationUnit
        private void AppendCompilationUnit(StringBuilder builder, string @namespace, string name, KeyValueTreeList<AddressableAssetEntry> treeList) {
            builder.AppendLine( $"namespace {@namespace} {{" );
            {
                AppendClass( builder, 1, name, treeList.Items.ToArray() );
            }
            builder.AppendLine( "}" );
        }
        private void AppendClass(StringBuilder builder, int indent, string name, KeyValueTreeList<AddressableAssetEntry>.Item[] items) {
            builder.AppendIndent( indent ).AppendLine( $"public static class @{name} {{" );
            foreach (var item in Sort( items )) {
                if (item is KeyValueTreeList<AddressableAssetEntry>.ValueItem @const) {
                    AppendConst( builder, indent + 1, Escape( @const.Key, name ), @const.Value );
                } else
                if (item is KeyValueTreeList<AddressableAssetEntry>.ListItem @class) {
                    AppendClass( builder, indent + 1, Escape( @class.Key, name ), @class.Items.ToArray() );
                }
            }
            builder.AppendIndent( indent ).AppendLine( "}" );
        }
        private void AppendConst(StringBuilder builder, int indent, string name, AddressableAssetEntry value) {
            if (value.IsAsset()) {
                builder.AppendIndent( indent ).AppendLine( $"public const string @{name} = \"{value.address}\";" );
            } else {
                throw Exceptions.Internal.NotSupported( $"Entry {value} is not supported" );
            }
        }

        // Sort
        public virtual IEnumerable<KeyValueTreeList<AddressableAssetEntry>.Item> Sort(IEnumerable<KeyValueTreeList<AddressableAssetEntry>.Item> items) {
            return items
                .OrderByDescending( i => i.Key.Equals( "UnityEngine" ) )
                .ThenByDescending( i => i.Key.Equals( "UnityEditor" ) )

                .ThenByDescending( i => i.Key.Equals( "EditorSceneList" ) )
                .ThenByDescending( i => i.Key.Equals( "Resources" ) )

                .ThenByDescending( i => i.Key.Equals( "Project" ) )
                .ThenByDescending( i => i.Key.Equals( "Presentation" ) )
                .ThenByDescending( i => i.Key.Equals( "UI" ) )
                .ThenByDescending( i => i.Key.Equals( "App" ) )
                .ThenByDescending( i => i.Key.Equals( "Domain" ) )
                .ThenByDescending( i => i.Key.Equals( "Entities" ) )
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
                .ThenByDescending( i => i.Key.Equals( "WorldScene" ) )
                .ThenByDescending( i => i.Key.Equals( "LevelScene" ) )
                .ThenByDescending( i => i.Key.Equals( "MainScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "GameScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "DebugScreen" ) )
                .ThenByDescending( i => i.Key.Equals( "Common" ) )

                .ThenBy( i => i.Key );
        }

        // IsSupported
        public virtual bool IsSupported(AddressableAssetEntry entry) {
            return entry.MainAssetType != typeof( DefaultAsset );
        }

        // Helpers
        private static string Escape(string name, string? outer) {
            //var chars = name.ToCharArray();
            //for (var i = 0; i < chars.Length; i++) {
            //    if (chars[ i ] is '_' || char.IsLetterOrDigit( chars[ i ] )) {
            //        continue;
            //    }
            //    chars[ i ] = '_';
            //}
            //name = new string( chars );
            name = name.TrimStart( ' ', '-', '_', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' );
            name = name.TrimEnd( ' ', '-', '_' );
            name = name.Replace( ' ', '_' );
            name = name.Replace( '-', '_' );
            name = name.Replace( '@', '_' );
            return name == outer ? (name + "_") : name;
        }
        private static void WriteText(string path, string text) {
            if (!File.Exists( path ) || File.ReadAllText( path ) != text) {
                File.WriteAllText( path, text );
                AssetDatabase.ImportAsset( path, ImportAssetOptions.Default );
            }
        }

    }
}
#endif

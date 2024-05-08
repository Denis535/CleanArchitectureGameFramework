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

    public class AddressableResourcesSourceGenerator {

        // Generate
        public virtual void Generate(AddressableAssetSettings settings, string path, string @namespace, string name) {
            var treeList = GetTreeList( GetEntries( settings ).Where( IsSupported ) );
            Generate( path, @namespace, name, treeList );
        }
        public virtual void Generate(string path, string @namespace, string name, KeyValueTreeList<AddressableAssetEntry> treeList) {
            var builder = new StringBuilder();
            builder.AppendCompilationUnit( @namespace, name, treeList );
            WriteText( path, builder.ToString() );
        }

        // IsSupported
        protected virtual bool IsSupported(AddressableAssetEntry entry) {
            return entry.MainAssetType != typeof( DefaultAsset );
        }

        // Helpers
        private static List<AddressableAssetEntry> GetEntries(AddressableAssetSettings settings) {
            var entries = new List<AddressableAssetEntry>();
            settings.GetAllAssets( entries, true );
            return entries;
        }
        // Helpers
        private static KeyValueTreeList<AddressableAssetEntry> GetTreeList(IEnumerable<AddressableAssetEntry> entries) {
            var treeList = new KeyValueTreeList<AddressableAssetEntry>();
            foreach (var entry in entries) {
                var path = GetPath( entry );
                treeList.AddValue( path.SkipLast( 1 ), path.Last(), entry );
            }
            return treeList;
        }
        private static string[] GetPath(AddressableAssetEntry entry) {
            if (IsAsset( entry )) {
                if (IsMainAsset( entry )) {
                    var path = GetAddressWithoutExtension( entry ).Split( '/', '\\', '.' );
                    var dir = path.SkipLast( 1 );
                    var name = path.Last();
                    if (name.Contains( " #" )) name = name.Substring( 0, name.IndexOf( " #" ) );
                    return dir.Append( name ).ToArray();
                } else {
                    var path = GetPath( entry.ParentEntry );
                    var dir = path.SkipLast( 1 );
                    var name = path.Last();
                    return dir.Append( name ).Append( entry.TargetAsset.name ).ToArray();
                }
            } else {
                throw new NotSupportedException( $"Entry {entry} is not supported" );
            }
        }
        private static string GetAddressWithoutExtension(AddressableAssetEntry entry) {
            if (Path.GetExtension( entry.address ) == Path.GetExtension( entry.AssetPath )) {
                return Path.ChangeExtension( entry.address, null );
            }
            return entry.address;
        }
        // Helpers
        private static bool IsAsset(AddressableAssetEntry entry) {
            return !entry.IsFolder;
        }
        private static bool IsMainAsset(AddressableAssetEntry entry) {
            if (!entry.IsFolder) {
                return entry.ParentEntry == null || entry.ParentEntry.IsFolder;
            }
            return false;
        }
        private static bool IsSubAsset(AddressableAssetEntry entry) {
            if (!entry.IsFolder) {
                return entry.ParentEntry != null && !entry.ParentEntry.IsFolder;
            }
            return false;
        }
        private static bool IsFolder(AddressableAssetEntry entry) {
            return entry.IsFolder;
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

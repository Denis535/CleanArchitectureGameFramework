#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.Tools_ {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    internal static class ResourcesSourceGeneratorHelper {

        // GetEntries
        public static List<AddressableAssetEntry> GetEntries(this AddressableAssetSettings settings) {
            var entries = new List<AddressableAssetEntry>();
            settings.GetAllAssets( entries, true );
            return entries;
        }

        // GetTreeList
        public static KeyValueTreeList<AddressableAssetEntry> GetTreeList(IEnumerable<AddressableAssetEntry> entries) {
            var treeList = new KeyValueTreeList<AddressableAssetEntry>();
            foreach (var entry in entries) {
                var path = GetPath( entry );
                treeList.AddValue( path.SkipLast( 1 ), path.Last(), entry );
            }
            return treeList;
        }
        private static string[] GetPath(AddressableAssetEntry entry) {
            if (entry.IsAsset()) {
                if (entry.IsMainAsset()) {
                    var dir = GetDir( entry );
                    var name = GetName( entry );
                    return dir.Append( name ).ToArray();
                } else {
                    var dir = GetDir( entry.ParentEntry );
                    var name = GetName( entry.ParentEntry );
                    var name2 = entry.TargetAsset.name;
                    return dir.Append( name + "__" + name2 ).ToArray();
                }
            } else {
                throw Exceptions.Internal.NotSupported( $"Entry {entry} is not supported" );
            }
        }
        private static string[] GetDir(AddressableAssetEntry entry) {
            return Path.GetDirectoryName( entry.address ).Split( '/', '\\', '.' );
        }
        private static string GetName(AddressableAssetEntry entry) {
            var name = Path.GetFileNameWithoutExtension( entry.address );
            if (name.Contains( " #" )) name = name.Substring( 0, name.IndexOf( " #" ) );
            if (name.Contains( " @" )) name = name.Substring( 0, name.IndexOf( " @" ) );
            return name;
        }

        // IsAsset
        public static bool IsAsset(this AddressableAssetEntry entry) {
            return !entry.IsFolder;
        }
        public static bool IsMainAsset(this AddressableAssetEntry entry) {
            if (!entry.IsFolder) {
                return entry.ParentEntry == null || entry.ParentEntry.IsFolder;
            }
            return false;
        }
        public static bool IsSubAsset(this AddressableAssetEntry entry) {
            if (!entry.IsFolder) {
                return entry.ParentEntry != null && !entry.ParentEntry.IsFolder;
            }
            return false;
        }
        public static bool IsFolder(this AddressableAssetEntry entry) {
            return entry.IsFolder;
        }

    }
}
#endif

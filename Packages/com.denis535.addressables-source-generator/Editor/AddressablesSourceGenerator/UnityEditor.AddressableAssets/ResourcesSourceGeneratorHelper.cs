#nullable enable
namespace UnityEditor.AddressableAssets {
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
                    var address = Path.ChangeExtension( entry.address, null );
                    if (address.Contains( " #" )) address = address.Substring( 0, address.IndexOf( " #" ) );
                    if (address.Contains( " @" )) address = address.Substring( 0, address.IndexOf( " @" ) );
                    return address.Split( '/', '\\', '.' );
                } else {
                    var result = GetPath( entry.ParentEntry );
                    result[ ^1 ] += "__" + entry.TargetAsset.name;
                    return result;
                }
            } else {
                throw new NotSupportedException( $"Entry {entry} is not supported" );
            }
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

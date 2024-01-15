#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.Tools_ {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal static class LabelsSourceGeneratorHelper {

        // GetTreeList
        public static KeyValueTreeList<string> GetTreeList(IEnumerable<string> labels) {
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

    }
}
#endif

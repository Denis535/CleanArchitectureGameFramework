#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public class ColorfulProjectWindow : ColorfulProjectWindowBase {

        // ModulePaths
        protected string[] ModulePaths { get; }

        // Constructor
        public ColorfulProjectWindow() {
            ModulePaths = AssetDatabase.GetAllAssetPaths()
                .Where( i => Path.GetExtension( i ) is ".asmdef" or ".asmref" )
                .Select( Path.GetDirectoryName )
                .Select( i => i.Replace( '\\', '/' ) )
                .Distinct()
                .OrderByDescending( i => i.StartsWith( "Assets/" ) )
                .ToArray();
        }
        public ColorfulProjectWindow(string[] modulePaths) {
            ModulePaths = modulePaths;
        }

        // OnGUI
        protected override void OnGUI(string guid, Rect rect) {
            base.OnGUI( guid, rect );
        }
        protected override void OnGUI(Rect rect, string path, string module, string content) {
            base.OnGUI( rect, path, module, content );
        }

        // Helpers
        protected override string? GetModulePath(string path) {
            return ModulePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
        }
        // Helpers
        protected override bool IsAssets(string path, string module, string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        protected override bool IsResources(string path, string module, string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        protected override bool IsSources(string path, string module, string content) {
            return AssetDatabase.IsValidFolder( path ) || content.Contains( '/' );
        }

    }
}

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

        // Settings
        protected ColorfulProjectWindowSettings Settings => ColorfulProjectWindowSettings.Instance;
        // ModulePaths
        protected string[] ModulePaths { get; }

        // Constructor
        public ColorfulProjectWindow() {
            ModulePaths = AssetDatabase.GetAllAssetPaths()
                .Where( i => Path.GetExtension( i ) is ".asmdef" or ".asmref" )
                .Select( Path.GetDirectoryName )
                .Select( i => i.Replace( '\\', '/' ) )
                .OrderByDescending( i => i.StartsWith( "Assets/" ) )
                .Distinct()
                .ToArray();
        }
        public ColorfulProjectWindow(string[] modulePaths) {
            ModulePaths = modulePaths;
        }

        // OnGUI
        protected override void OnGUI(string guid, Rect rect) {
            base.OnGUI( guid, rect );
        }

        // DrawItem
        protected override void DrawItem(Rect rect, string path) {
            base.DrawItem( rect, path );
        }
        protected override void DrawModule(Rect rect, string path, string module) {
            DrawItem( rect, Settings.ModuleColor, 0 );
        }
        protected override void DrawContent(Rect rect, string path, string module, string content) {
            if (content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." )) {
                var depth = content.Count( i => i == '/' );
                DrawItem( rect, Settings.AssetsColor, depth );
                return;
            }
            if (content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." )) {
                var depth = content.Count( i => i == '/' );
                DrawItem( rect, Settings.ResourcesColor, depth );
                return;
            }
            {
                if (AssetDatabase.IsValidFolder( path ) || Path.GetExtension( path ) is ".cs") {
                    var depth = content.Count( i => i == '/' );
                    DrawItem( rect, Settings.ContentColor, depth );
                } else
                if (Path.GetExtension( path ) is not ".asmdef" and not ".asmref" and not ".rsp") {
                    var depth = content.Count( i => i == '/' );
                    DrawItem( rect, Settings.ContentColor, depth );
                }
            }
        }

        // GetModulePath
        protected override string? GetModulePath(string path) {
            return ModulePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
        }

    }
}

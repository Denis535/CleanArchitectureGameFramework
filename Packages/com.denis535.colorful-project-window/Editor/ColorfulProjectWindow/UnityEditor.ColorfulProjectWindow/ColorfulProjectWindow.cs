#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
                    .OrderByDescending( i => i.StartsWith( "Assets/" ) )
                    .Distinct()
                    .ToArray();
        }
        public ColorfulProjectWindow(string[] modulePaths) {
            ModulePaths = modulePaths;
        }

        // OnGUI
        protected override void OnGUI(Rect rect, string path) {
            base.OnGUI( rect, path );
        }

        // IsModule
        protected override bool IsModule(string path, [NotNullWhen( true )] out string? module, [NotNullWhen( true )] out string? content) {
            var modulePath = ModulePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (modulePath != null) {
                module = Path.GetFileName( modulePath );
                content = path.Substring( modulePath.Length ).TrimStart( '/' );
                return true;
            }
            module = null;
            content = null;
            return false;
        }
        protected override bool IsContent(string path, string module, string content) {
            return base.IsContent( path, module, content );
        }

        // DrawModule
        protected override void DrawModule(Rect rect, string path, string module) {
            DrawItem( rect, ColorfulProjectWindowSettings.Instance.ModuleColor, 0 );
        }
        protected override void DrawContent(Rect rect, string path, string module, string content) {
            if (IsAssets( path, module, content )) {
                DrawItem( rect, ColorfulProjectWindowSettings.Instance.AssetsColor, content.Count( i => i == '/' ) );
                return;
            }
            if (IsResources( path, module, content )) {
                DrawItem( rect, ColorfulProjectWindowSettings.Instance.ResourcesColor, content.Count( i => i == '/' ) );
                return;
            }
            if (IsSources( path, module, content )) {
                DrawItem( rect, ColorfulProjectWindowSettings.Instance.SourcesColor, content.Count( i => i == '/' ) );
                return;
            }
        }

        // Heleprs
        protected static bool IsAssets(string path, string module, string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        protected static bool IsResources(string path, string module, string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        protected static bool IsSources(string path, string module, string content) {
            return true;
        }

    }
}

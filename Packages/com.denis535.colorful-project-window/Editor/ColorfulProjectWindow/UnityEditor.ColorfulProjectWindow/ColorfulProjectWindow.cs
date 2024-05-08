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

    [InitializeOnLoad]
    public class ColorfulProjectWindow {

        private readonly string[] ModulePaths;

        private ColorfulProjectWindowSettings Settings => ColorfulProjectWindowSettings.Instance;

        // ProjectWindow
        static ColorfulProjectWindow() {
            new ColorfulProjectWindow();
        }

        // Constructor
        public ColorfulProjectWindow() {
            ModulePaths = AssetDatabase.GetAllAssetPaths().Where( i => Path.GetExtension( i ) is ".asmdef" or ".asmref" ).Select( Path.GetDirectoryName ).Select( i => i.Replace( '\\', '/' ) ).ToArray();
            EditorApplication.projectWindowItemOnGUI = OnGUI;
        }

        // OnGUI
        private void OnGUI(string guid, Rect rect) {
            var path = AssetDatabase.GUIDToAssetPath( guid );
            var modulePath = ModulePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (modulePath != null) {
                var module = Path.GetFileName( modulePath );
                var content = path.Substring( modulePath.Length ).TrimStart( '/' );
                OnGUI( rect, path, module, content );
            }
        }
        private void OnGUI(Rect rect, string path, string module, string content) {
            if (string.IsNullOrEmpty( content )) {
                DrawModule( rect );
                return;
            }
            if (AssetDatabase.IsValidFolder( path ) || content.Contains( '/' )) {
                if (IsAssets( content )) {
                    DrawAssets( rect, content );
                    return;
                }
                if (IsResources( content )) {
                    DrawResources( rect, content );
                    return;
                }
                if (IsSources( content )) {
                    DrawSources( rect, content );
                    return;
                }
            }
        }

        // Helpers
        private bool IsAssets(string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        private bool IsResources(string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        private bool IsSources(string content) {
            return true;
        }
        // Helpers
        private void DrawModule(Rect rect) {
            DrawItem( rect, Settings.ModuleColor, 0 );
        }
        private void DrawAssets(Rect rect, string content) {
            var depth = content.Count( i => i == '/' );
            DrawItem( rect, Settings.AssetsColor, depth );
        }
        private void DrawResources(Rect rect, string content) {
            var depth = content.Count( i => i == '/' );
            DrawItem( rect, Settings.ResourcesColor, depth );
        }
        private void DrawSources(Rect rect, string content) {
            var depth = content.Count( i => i == '/' );
            DrawItem( rect, Settings.SourcesColor, depth );
        }
        // Helpers
        private static void DrawItem(Rect rect, Color color, int depth) {
            if (rect.height == 16) {
                rect.x -= 16;
                rect.width = 16;
                rect.height = 16;
                color = depth switch {
                    0 => color,
                    _ => Darken( color, 2.5f ),
                };
                DrawRect( rect, color );
            } else {
                rect.width = 64;
                rect.height = 64;
                color = depth switch {
                    0 => color,
                    _ => Darken( color, 2.5f ),
                };
                DrawRect( rect, color );
            }
        }
        private static void DrawRect(Rect rect, Color color) {
            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = prev;
        }
        //private static Color Lighten(Color color, float factor) {
        //    Color.RGBToHSV( color, out var h, out var s, out var v );
        //    var result = Color.HSVToRGB( h, s, v * factor );
        //    result.a = color.a;
        //    return result;
        //}
        private static Color Darken(Color color, float factor) {
            Color.RGBToHSV( color, out var h, out var s, out var v );
            var result = Color.HSVToRGB( h, s, v / factor );
            result.a = color.a;
            return result;
        }

    }
}

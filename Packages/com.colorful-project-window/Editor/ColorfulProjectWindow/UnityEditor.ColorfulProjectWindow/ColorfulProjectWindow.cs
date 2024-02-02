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
        public void OnGUI(string guid, Rect rect) {
            var path = AssetDatabase.GUIDToAssetPath( guid );
            var module = ModulePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (module != null) {
                if (path == module) {
                    DrawModule( rect );
                    return;
                }
                var content = path.Substring( module.Length );
                if (AssetDatabase.IsValidFolder( path ) || content.Skip( 1 ).Contains( '/' )) {
                    if (content.Equals( "/Assets" ) || content.StartsWith( "/Assets/" ) || content.StartsWith( "/Assets." )) {
                        DrawAssets( rect, content.Skip( 1 ).Count( i => i == '/' ) );
                        return;
                    }
                    if (content.Equals( "/Resources" ) || content.StartsWith( "/Resources/" ) || content.StartsWith( "/Resources." )) {
                        DrawResources( rect, content.Skip( 1 ).Count( i => i == '/' ) );
                        return;
                    }
                    {
                        DrawSources( rect, content.Skip( 1 ).Count( i => i == '/' ) );
                        return;
                    }
                }
            }
        }

        // Helpers
        private void DrawModule(Rect rect) {
            DrawItem( rect, Settings.ModuleColor, 0 );
        }
        private void DrawAssets(Rect rect, int depth) {
            DrawItem( rect, Settings.AssetsColor, depth );
        }
        private void DrawResources(Rect rect, int depth) {
            DrawItem( rect, Settings.ResourcesColor, depth );
        }
        private void DrawSources(Rect rect, int depth) {
            DrawItem( rect, Settings.SourcesColor, depth );
        }
        // Helpers
        private static void DrawItem(Rect rect, Color color, int depth) {
            rect.x -= 16;
            rect.width = 16;
            color = depth switch {
                0 => color,
                _ => Darken( color, 2.5f ),
            };
            DrawRect( rect, color );
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

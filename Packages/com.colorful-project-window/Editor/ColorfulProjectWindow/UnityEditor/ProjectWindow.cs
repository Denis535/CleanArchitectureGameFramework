#nullable enable
namespace UnityEditor {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public static class ProjectWindow {

        private static string[] Modules { get; }

        // ProjectWindow
        static ProjectWindow() {
            Modules = AssetDatabase.GetAllAssetPaths().Where( i => Path.GetExtension( i ) is ".asmdef" or ".asmref" ).Select( Path.GetDirectoryName ).Select( i => i.Replace( '\\', '/' ) ).ToArray();
            EditorApplication.projectWindowItemOnGUI = OnGUI;
        }

        // OnGUI
        public static void OnGUI(string guid, Rect rect) {
            var path = AssetDatabase.GUIDToAssetPath( guid );
            var module = Modules.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (module != null) {
                if (path == module) {
                    DrawModule( rect );
                    return;
                }
                var content = path.Substring( module.Length );
                if (AssetDatabase.IsValidFolder( path ) || content.Skip( 1 ).Contains( '/' )) {
                    if (content.Equals( "/Assets" ) || content.StartsWith( "/Assets/" ) || content.StartsWith( "/Assets." )) {
                        var depth = content.Count( i => i == '/' ) - 1;
                        DrawAssets( rect, depth );
                        return;
                    }
                    if (content.Equals( "/Resources" ) || content.StartsWith( "/Resources/" ) || content.StartsWith( "/Resources." )) {
                        var depth = content.Count( i => i == '/' ) - 1;
                        DrawAssets( rect, depth );
                        return;
                    }
                    {
                        var depth = content.Count( i => i == '/' ) - 1;
                        DrawSources( rect, depth );
                        return;
                    }
                }
            }
        }

        // Helpers
        private static void DrawModule(Rect rect) {
            var color = HSVA( 000, 1, 1, 0.3f );
            DrawItem( rect, color );
        }
        private static void DrawAssets(Rect rect, int depth) {
            var color = depth switch {
                0 => HSVA( 060, 1f, 1.0f, 0.3f ),
                _ => HSVA( 060, 1f, 0.4f, 0.3f ),
            };
            DrawItem( rect, color );
        }
        private static void DrawSources(Rect rect, int depth) {
            var color = depth switch {
                0 => HSVA( 120, 1f, 1.0f, 0.3f ),
                _ => HSVA( 120, 1f, 0.4f, 0.3f ),
            };
            DrawItem( rect, color );
        }
        private static void DrawItem(Rect rect, Color color) {
            rect.x -= 16;
            rect.width = 16;
            DrawRect( rect, color );
        }
        // Helpers
        private static Color HSVA(int h, float s, float v, float a) {
            var color = Color.HSVToRGB( h / 360f, s, v );
            color.a = a;
            return color;
        }
        private static void DrawRect(Rect rect, Color color) {
            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = prev;
        }

    }
}

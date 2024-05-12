#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public abstract class ColorfulProjectWindowBase {

        // Constructor
        public ColorfulProjectWindowBase() {
            EditorApplication.projectWindowItemOnGUI = OnGUI;
        }

        // OnGUI
        protected virtual void OnGUI(string guid, Rect rect) {
            DrawItem( rect, AssetDatabase.GUIDToAssetPath( guid ) );
        }

        // DrawItem
        protected virtual void DrawItem(Rect rect, string path) {
            var modulePath = GetModulePath( path );
            if (modulePath != null) {
                var module = Path.GetFileName( modulePath );
                var content = path.Substring( modulePath.Length ).TrimStart( '/' );
                if (string.IsNullOrEmpty( content )) {
                    DrawModule( rect, path, module );
                } else {
                    DrawContent( rect, path, module, content );
                }
            }
        }
        protected abstract void DrawModule(Rect rect, string path, string module);
        protected abstract void DrawContent(Rect rect, string path, string module, string content);

        // GetModulePath
        protected abstract string? GetModulePath(string path);

        // Helpers
        protected static void DrawItem(Rect rect, Color color, int depth) {
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
        protected static void DrawRect(Rect rect, Color color) {
            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = prev;
        }
        // Helpers
        protected static Color Lighten(Color color, float factor) {
            Color.RGBToHSV( color, out var h, out var s, out var v );
            var result = Color.HSVToRGB( h, s, v * factor );
            result.a = color.a;
            return result;
        }
        protected static Color Darken(Color color, float factor) {
            Color.RGBToHSV( color, out var h, out var s, out var v );
            var result = Color.HSVToRGB( h, s, v / factor );
            result.a = color.a;
            return result;
        }

    }
}

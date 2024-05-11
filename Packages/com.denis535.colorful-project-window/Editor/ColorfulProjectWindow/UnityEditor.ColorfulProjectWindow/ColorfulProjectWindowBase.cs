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

    public abstract class ColorfulProjectWindowBase {

        // Instance
        protected static ColorfulProjectWindowBase? Instance { get; private set; }
        // Settings
        protected ColorfulProjectWindowSettings Settings => ColorfulProjectWindowSettings.Instance;

        // Constructor
        public ColorfulProjectWindowBase() {
            if (Instance != null) throw new InvalidOperationException( "There is already 'ColorfulProjectWindowBase' instance" );
            Instance = this;
            EditorApplication.projectWindowItemOnGUI = OnGUI;
        }

        // OnGUI
        protected virtual void OnGUI(string guid, Rect rect) {
            var path = AssetDatabase.GUIDToAssetPath( guid );
            var modulePath = GetModulePath( path );
            if (modulePath != null) {
                var module = Path.GetFileName( modulePath );
                var content = path.Substring( modulePath.Length ).TrimStart( '/' );
                OnGUI( rect, path, module, content );
            }
        }
        protected virtual void OnGUI(Rect rect, string path, string module, string content) {
            if (string.IsNullOrEmpty( content )) {
                DrawModule( rect, path, module );
                return;
            }
            if (IsAssets( path, module, content )) {
                DrawAssets( rect, path, module, content );
                return;
            }
            if (IsResources( path, module, content )) {
                DrawResources( rect, path, module, content );
                return;
            }
            if (IsSources( path, module, content )) {
                DrawSources( rect, path, module, content );
                return;
            }
        }

        // Helpers
        protected abstract string? GetModulePath(string path);
        // Helpers
        protected abstract bool IsAssets(string path, string module, string content);
        protected abstract bool IsResources(string path, string module, string content);
        protected abstract bool IsSources(string path, string module, string content);
        // Helpers
        protected virtual void DrawModule(Rect rect, string path, string module) {
            DrawItem( rect, Settings.ModuleColor, 0 );
        }
        protected virtual void DrawAssets(Rect rect, string path, string module, string content) {
            var depth = content.Count( i => i == '/' );
            DrawItem( rect, Settings.AssetsColor, depth );
        }
        protected virtual void DrawResources(Rect rect, string path, string module, string content) {
            var depth = content.Count( i => i == '/' );
            DrawItem( rect, Settings.ResourcesColor, depth );
        }
        protected virtual void DrawSources(Rect rect, string path, string module, string content) {
            var depth = content.Count( i => i == '/' );
            DrawItem( rect, Settings.SourcesColor, depth );
        }
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
        //protected static Color Lighten(Color color, float factor) {
        //    Color.RGBToHSV( color, out var h, out var s, out var v );
        //    var result = Color.HSVToRGB( h, s, v * factor );
        //    result.a = color.a;
        //    return result;
        //}
        protected static Color Darken(Color color, float factor) {
            Color.RGBToHSV( color, out var h, out var s, out var v );
            var result = Color.HSVToRGB( h, s, v / factor );
            result.a = color.a;
            return result;
        }

    }
}

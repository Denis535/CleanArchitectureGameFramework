#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public abstract class ProjectWindowBase : IDisposable {

        // Constructor
        public ProjectWindowBase() {
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }
        public virtual void Dispose() {
            EditorApplication.projectWindowItemOnGUI -= OnGUI;
        }

        // OnGUI
        private void OnGUI(string guid, Rect rect) {
            OnGUI( rect, AssetDatabase.GUIDToAssetPath( guid ) );
        }
        protected virtual void OnGUI(Rect rect, string path) {
            if (IsPackage( path, out var package, out var content )) {
                DrawPackage( rect, path, package, content );
            }
            if (IsAssembly( path, out var assembly, out content )) {
                DrawAssembly( rect, path, assembly, content );
            }
        }

        // IsPackage
        protected abstract bool IsPackage(string path, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content);
        protected abstract bool IsAssembly(string path, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content);

        // DrawPackage
        protected abstract void DrawPackage(Rect rect, string path, string package, string content);
        protected abstract void DrawAssembly(Rect rect, string path, string assembly, string content);

        // Helpers
        protected static void DrawItem(Rect rect, Color color, int depth) {
            if (rect.height == 16) {
                rect.x -= 16;
                rect.width = 16;
                rect.height = 16;
                color = depth switch {
                    0 => color,
                    _ => Darken( color, 1.5f ),
                };
                DrawRect( rect, color );
            } else {
                rect.width = 64;
                rect.height = 64;
                color = depth switch {
                    0 => color,
                    _ => Darken( color, 1.5f ),
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
        protected static Color HSVA(int h, float s, float v, float a) {
            var color = Color.HSVToRGB( h / 360f, s, v );
            color.a = a;
            return color;
        }
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

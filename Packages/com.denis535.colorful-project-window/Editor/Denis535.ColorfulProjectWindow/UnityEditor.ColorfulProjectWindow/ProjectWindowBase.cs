#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public abstract class ProjectWindowBase : IDisposable {

        // Settings
        protected Settings Settings => Settings.Instance;

        // Constructor
        public ProjectWindowBase() {
            EditorApplication.projectWindowItemOnGUI = OnGUI;
        }
        public virtual void Dispose() {
            EditorApplication.projectWindowItemOnGUI = null;
        }

        // OnGUI
        protected virtual void OnGUI(string guid, Rect rect) {
            var path = AssetDatabase.GUIDToAssetPath( guid );
            DrawElement( rect, path );
        }

        // DrawElement
        protected virtual void DrawElement(Rect rect, string path) {
            if (IsPackage( path, out var package, out var content )) {
                if (content == string.Empty) {
                    // Assets/[Package]
                    // Assets/[Package]
                    DrawPackage( rect, path, package );
                } else {
                    if (IsAssembly( path, package, out var assembly, out content )) {
                        // Assets/[Package]/[Assembly]/[Content]
                        // Assets/[Package]/[Assembly]/[Content]
                        DrawAssembly( rect, path, package, assembly, content );
                    }
                }
            } else {
                if (IsAssembly( path, null, out var assembly, out content )) {
                    // Assets/[Assembly]/[Content]
                    // Assets/[Assembly]/[Content]
                    DrawAssembly( rect, path, null, assembly, content );
                }
            }
        }
        private void DrawAssembly(Rect rect, string path, string? package, string assembly, string content) {
            if (content == string.Empty) {
                DrawAssembly( rect, path, package, assembly );
            } else {
                if (IsAssets( path, package, assembly, content )) {
                    DrawAssets( rect, path, package, assembly, content );
                } else if (IsResources( path, package, assembly, content )) {
                    DrawResources( rect, path, package, assembly, content );
                } else if (IsSources( path, package, assembly, content )) {
                    DrawSources( rect, path, package, assembly, content );
                }
            }
        }

        // DrawPackage
        protected virtual void DrawPackage(Rect rect, string path, string package) {
            Highlight( rect, Settings.PackageColor, false );
        }
        protected virtual void DrawAssembly(Rect rect, string path, string? package, string assembly) {
            Highlight( rect, Settings.AssemblyColor, false );
        }
        protected virtual void DrawAssets(Rect rect, string path, string? package, string? assembly, string content) {
            Highlight( rect, Settings.AssetsColor, content.Contains( '/' ) );
        }
        protected virtual void DrawResources(Rect rect, string path, string? package, string? assembly, string content) {
            Highlight( rect, Settings.ResourcesColor, content.Contains( '/' ) );
        }
        protected virtual void DrawSources(Rect rect, string path, string? package, string? assembly, string content) {
            Highlight( rect, Settings.SourcesColor, content.Contains( '/' ) );
        }

        // IsPackage
        protected abstract bool IsPackage(string path, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content);
        protected abstract bool IsAssembly(string path, string? package, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content);
        protected virtual bool IsAssets(string path, string? package, string? assembly, string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        protected virtual bool IsResources(string path, string? package, string? assembly, string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        protected virtual bool IsSources(string path, string? package, string? assembly, string content) {
            return !path.EndsWith( ".asmdef" ) && !path.EndsWith( ".asmref" ) && !path.EndsWith( ".rsp" );
        }

        // Helpers
        protected static bool IsMatch(string path, string pattern, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? content) {
            if (path.Equals( pattern ) || path.StartsWith( pattern + '/' )) {
                name = Path.GetFileName( pattern );
                content = path.Substring( pattern.Length ).TrimStart( '/' );
                return true;
            }
            name = null;
            content = null;
            return false;
        }
        protected static void Highlight(Rect rect, Color color, bool isDark) {
            if (rect.height == 16) {
                rect.x -= 16;
                rect.width = 16;
                rect.height = 16;
            } else {
                rect.width = 64;
                rect.height = 64;
            }
            if (isDark) {
                color = Darken( color, 1.5f );
            }
            DrawRect( rect, color );
        }
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
        protected static void DrawRect(Rect rect, Color color) {
            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = prev;
        }
        protected static bool IsFile(string path) {
            return !AssetDatabase.IsValidFolder( path );
        }
        protected static bool IsFolder(string path) {
            return AssetDatabase.IsValidFolder( path );
        }

    }
}

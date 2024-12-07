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
            // Assets/[Package]/[Assembly]/[Content]
            // Assets/[Package]/[Assembly]/[Content]

            // Assets/[Assembly]/[Content]
            // Assets/[Assembly]/[Content]

            if (IsAssembly( path, out var assembly, out var content )) {
                DrawAssemblyElement( rect, path, assembly, content );
            } else if (IsPackage( path, out var package, out content )) {
                DrawPackageElement( rect, path, package, content );
            }
        }
        protected virtual void DrawPackageElement(Rect rect, string path, string package, string content) {
            if (content == string.Empty) {
                DrawPackage( rect, path, package );
            }
        }
        protected virtual void DrawAssemblyElement(Rect rect, string path, string assembly, string content) {
            if (content == string.Empty) {
                DrawAssembly( rect, path, assembly );
            } else {
                if (IsFolder( path ) || content.Contains( '/' )) {
                    if (IsAssets( path, assembly, content )) {
                        DrawAssets( rect, path, assembly, content );
                    } else if (IsResources( path, assembly, content )) {
                        DrawResources( rect, path, assembly, content );
                    } else if (IsSources( path, assembly, content )) {
                        DrawSources( rect, path, assembly, content );
                    }
                }
            }
        }

        // DrawPackage
        protected virtual void DrawPackage(Rect rect, string path, string package) {
            Highlight( rect, Settings.PackageColor, false );
        }
        protected virtual void DrawAssembly(Rect rect, string path, string assembly) {
            Highlight( rect, Settings.AssemblyColor, false );
        }
        protected virtual void DrawAssets(Rect rect, string path, string assembly, string content) {
            Highlight( rect, Settings.AssetsColor, content.Contains( '/' ) );
        }
        protected virtual void DrawResources(Rect rect, string path, string assembly, string content) {
            Highlight( rect, Settings.ResourcesColor, content.Contains( '/' ) );
        }
        protected virtual void DrawSources(Rect rect, string path, string assembly, string content) {
            Highlight( rect, Settings.SourcesColor, content.Contains( '/' ) );
        }

        // IsPackage
        protected abstract bool IsPackage(string path, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content);
        protected abstract bool IsAssembly(string path, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content);
        protected virtual bool IsAssets(string path, string assembly, string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        protected virtual bool IsResources(string path, string assembly, string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        protected virtual bool IsSources(string path, string assembly, string content) {
            return true;
        }

        // Helpers
        protected static bool IsPackage(string path, string packagePath, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content) {
            if (path.Equals( packagePath ) || path.StartsWith( packagePath + '/' )) {
                package = Path.GetFileName( packagePath );
                content = path.Substring( packagePath.Length ).TrimStart( '/' );
                return true;
            }
            package = null;
            content = null;
            return false;
        }
        protected static bool IsAssembly(string path, string assemblyPath, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content) {
            if (path.Equals( assemblyPath ) || path.StartsWith( assemblyPath + '/' )) {
                assembly = Path.GetFileName( assemblyPath );
                content = path.Substring( assemblyPath.Length ).TrimStart( '/' );
                return true;
            }
            assembly = null;
            content = null;
            return false;
        }
        // Helpers
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
        // Helpers
        protected static void DrawRect(Rect rect, Color color) {
            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = prev;
        }
        // Helpers
        protected static bool IsFile(string path) {
            return !AssetDatabase.IsValidFolder( path );
        }
        protected static bool IsFolder(string path) {
            return AssetDatabase.IsValidFolder( path );
        }

    }
}

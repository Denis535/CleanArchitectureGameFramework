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
        protected abstract void DrawPackage(Rect rect, string path, string package);
        protected abstract void DrawAssembly(Rect rect, string path, string assembly);
        protected abstract void DrawAssets(Rect rect, string path, string assembly, string content);
        protected abstract void DrawResources(Rect rect, string path, string assembly, string content);
        protected abstract void DrawSources(Rect rect, string path, string assembly, string content);

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
        protected static bool IsFile(string path) {
            return !AssetDatabase.IsValidFolder( path );
        }
        protected static bool IsFolder(string path) {
            return AssetDatabase.IsValidFolder( path );
        }

    }
}

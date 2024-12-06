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
            // Assets/Package/Assembly/Namespace
            // Assets/Package/Assembly/Assets.Namespace
            if (IsAssembly( path, out var name, out var rest )) {
                // .../[Assembly]
                // .../[Assembly]/[Rest]
                DrawAssemblyElement( rect, path, name, rest );
            } else if (IsPackage( path, out name, out rest )) {
                // .../[Package]
                // .../[Package]/[Rest]
                DrawPackageElement( rect, path, name, rest );
            }
        }
        protected virtual void DrawPackageElement(Rect rect, string path, string name, string rest) {
            if (rest == string.Empty) {
                DrawPackage( rect, path, name );
            }
        }
        protected virtual void DrawAssemblyElement(Rect rect, string path, string name, string rest) {
            if (rest == string.Empty) {
                DrawAssembly( rect, path, name );
            } else {
                if (IsFolder( path ) || rest.Contains( '/' )) {
                    if (IsAssets( path, name, rest )) {
                        DrawAssets( rect, path, name, rest );
                    } else if (IsResources( path, name, rest )) {
                        DrawResources( rect, path, name, rest );
                    } else if (IsSources( path, name, rest )) {
                        DrawSources( rect, path, name, rest );
                    }
                }
            }
        }

        // DrawPackage
        protected abstract void DrawPackage(Rect rect, string path, string name);
        protected abstract void DrawAssembly(Rect rect, string path, string name);
        protected abstract void DrawAssets(Rect rect, string path, string name, string rest);
        protected abstract void DrawResources(Rect rect, string path, string name, string rest);
        protected abstract void DrawSources(Rect rect, string path, string name, string rest);

        // IsPackage
        protected abstract bool IsPackage(string path, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? rest);
        protected abstract bool IsAssembly(string path, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? rest);
        protected virtual bool IsAssets(string path, string name, string rest) {
            return rest.Equals( "Assets" ) || rest.StartsWith( "Assets/" ) || rest.StartsWith( "Assets." );
        }
        protected virtual bool IsResources(string path, string name, string rest) {
            return rest.Equals( "Resources" ) || rest.StartsWith( "Resources/" ) || rest.StartsWith( "Resources." );
        }
        protected virtual bool IsSources(string path, string name, string rest) {
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

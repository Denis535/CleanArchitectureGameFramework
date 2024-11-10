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
            if (IsAssembly( path, out var name, out var rest )) {
                DrawAssemblyElement( rect, path, name, rest );
            } else
            if (IsPackage( path, out name, out rest )) {
                DrawPackageElement( rect, path, name, rest );
            }
        }

        // DrawElement
        protected virtual void DrawPackageElement(Rect rect, string path, string name, string rest) {
            if (rest == string.Empty) {
                DrawPackageItem( rect, path, name );
            }
        }
        protected virtual void DrawAssemblyElement(Rect rect, string path, string name, string rest) {
            if (rest == string.Empty) {
                DrawAssemblyItem( rect, path, name );
            } else {
                DrawAssemblyContentElement( rect, path, name, rest );
            }
        }
        protected virtual void DrawAssemblyContentElement(Rect rect, string path, string name, string rest) {
            if (IsFile( path ) && !rest.Contains( '/' )) {
                return;
            }
            if (Path.GetExtension( path ) is ".asmdef" or ".asmref" or ".rsp") {
                return;
            }
            if (IsAssets( path, name, rest )) {
                DrawAssetsItem( rect, path, name, rest );
            } else
            if (IsResources( path, name, rest )) {
                DrawResourcesItem( rect, path, name, rest );
            } else
            if (IsSources( path, name, rest )) {
                DrawSourcesItem( rect, path, name, rest );
            }
        }

        // DrawItem
        protected abstract void DrawPackageItem(Rect rect, string path, string name);
        protected abstract void DrawAssemblyItem(Rect rect, string path, string name);
        protected abstract void DrawAssetsItem(Rect rect, string path, string name, string rest);
        protected abstract void DrawResourcesItem(Rect rect, string path, string name, string rest);
        protected abstract void DrawSourcesItem(Rect rect, string path, string name, string rest);

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

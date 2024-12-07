#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public class ProjectWindow : ProjectWindowBase {

        // Constructor
        static ProjectWindow() {
            new ProjectWindow();
        }

        // Constructor
        public ProjectWindow() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnGUI
        protected override void OnGUI(string guid, Rect rect) {
            base.OnGUI( guid, rect );
        }

        // DrawElement
        protected override void DrawElement(Rect rect, string path) {
            if (path.Equals( "Assets/Assets" ) || path.StartsWith( "Assets/Assets/" )) {
                Highlight( rect, Settings.AssetsColor, path.Count( i => i == '/' ) >= 2 );
                return;
            }
            if (path.StartsWith( "Assets/Assets." )) {
                Highlight( rect, Settings.AssetsColor, path.Count( i => i == '/' ) >= 2 );
                return;
            }
            base.DrawElement( rect, path );
        }
        protected override void DrawPackageElement(Rect rect, string path, string package, string content) {
            base.DrawPackageElement( rect, path, package, content );
        }
        protected override void DrawAssemblyElement(Rect rect, string path, string assembly, string content) {
            base.DrawAssemblyElement( rect, path, assembly, content );
        }

        // DrawPackage
        protected override void DrawPackage(Rect rect, string path, string package) {
            base.DrawPackage( rect, path, package );
        }
        protected override void DrawAssembly(Rect rect, string path, string assembly) {
            base.DrawAssembly( rect, path, assembly );
        }
        protected override void DrawAssets(Rect rect, string path, string assembly, string content) {
            base.DrawAssets( rect, path, assembly, content );
        }
        protected override void DrawResources(Rect rect, string path, string assembly, string content) {
            base.DrawResources( rect, path, assembly, content );
        }
        protected override void DrawSources(Rect rect, string path, string assembly, string content) {
            base.DrawSources( rect, path, assembly, content );
        }

        // IsPackage
        protected override bool IsPackage(string path, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content) {
            if (IsPackage( path, "Packages/com.denis535.addressables-extensions", out package, out content )) {
                return true;
            }
            if (IsPackage( path, "Packages/com.denis535.addressables-source-generator", out package, out content )) {
                return true;
            }
            if (IsPackage( path, "Packages/com.denis535.clean-architecture-game-framework", out package, out content )) {
                return true;
            }
            if (IsPackage( path, "Packages/com.denis535.colorful-project-window", out package, out content )) {
                return true;
            }
            package = null;
            content = null;
            return false;
        }
        protected override bool IsAssembly(string path, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content) {
            if (IsAssembly( path, "Assets/MyProject", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Assets/MyProject2", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.addressables-extensions/Runtime/Denis535.Addressables.Extensions", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.addressables-source-generator/Editor/Denis535.Addressables.SourceGenerator", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.clean-architecture-game-framework/Runtime/Denis535.CleanArchitectureGameFramework", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.clean-architecture-game-framework/Runtime/Denis535.CleanArchitectureGameFramework.Additions", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.clean-architecture-game-framework/Runtime/Denis535.CleanArchitectureGameFramework.Internal", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.clean-architecture-game-framework/Editor/Denis535.CleanArchitectureGameFramework.Editor", out assembly, out content )) {
                return true;
            }
            if (IsAssembly( path, "Packages/com.denis535.colorful-project-window/Editor/Denis535.ColorfulProjectWindow", out assembly, out content )) {
                return true;
            }
            assembly = null;
            content = null;
            return false;
        }
        protected override bool IsAssets(string path, string assembly, string content) {
            return base.IsAssets( path, assembly, content );
        }
        protected override bool IsResources(string path, string assembly, string content) {
            return base.IsResources( path, assembly, content );
        }
        protected override bool IsSources(string path, string assembly, string content) {
            return base.IsSources( path, assembly, content );
        }

    }
}
#endif

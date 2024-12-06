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
    public class ProjectWindow : ProjectWindowBase2 {

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
            return base.IsPackage( path, out package, out content );
        }
        protected override bool IsAssembly(string path, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content) {
            return base.IsAssembly( path, out assembly, out content );
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

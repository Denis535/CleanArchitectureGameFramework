#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
            base.DrawElement( rect, path );
        }
        protected override void DrawPackageElement(Rect rect, string path, string name, string rest) {
            base.DrawPackageElement( rect, path, name, rest );
        }
        protected override void DrawAssemblyElement(Rect rect, string path, string name, string rest) {
            base.DrawAssemblyElement( rect, path, name, rest );
        }

        // DrawPackage
        protected override void DrawPackage(Rect rect, string path, string name) {
            base.DrawPackage( rect, path, name );
        }
        protected override void DrawAssembly(Rect rect, string path, string name) {
            base.DrawAssembly( rect, path, name );
        }
        protected override void DrawAssets(Rect rect, string path, string name, string rest) {
            base.DrawAssets( rect, path, name, rest );
        }
        protected override void DrawResources(Rect rect, string path, string name, string rest) {
            base.DrawResources( rect, path, name, rest );
        }
        protected override void DrawSources(Rect rect, string path, string name, string rest) {
            base.DrawSources( rect, path, name, rest );
        }

        // IsPackage
        protected override bool IsPackage(string path, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? rest) {
            return base.IsPackage( path, out name, out rest );
        }
        protected override bool IsAssembly(string path, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? rest) {
            return base.IsAssembly( path, out name, out rest );
        }
        protected override bool IsAssets(string path, string name, string rest) {
            return base.IsAssets( path, name, rest );
        }
        protected override bool IsResources(string path, string name, string rest) {
            return base.IsResources( path, name, rest );
        }
        protected override bool IsSources(string path, string name, string rest) {
            return base.IsSources( path, name, rest );
        }

    }
}
#endif

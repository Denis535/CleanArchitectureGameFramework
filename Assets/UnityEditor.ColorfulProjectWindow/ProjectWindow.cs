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

        // DrawPackageElement
        protected override void DrawPackageElement(Rect rect, string path, string name, string rest) {
            base.DrawPackageElement( rect, path, name, rest );
        }
        protected override void DrawAssemblyElement(Rect rect, string path, string name, string rest) {
            base.DrawAssemblyElement( rect, path, name, rest );
        }
        protected override void DrawAssemblyContentElement(Rect rect, string path, string name, string rest) {
            base.DrawAssemblyContentElement( rect, path, name, rest );
        }

        // DrawPackageItem
        protected override void DrawPackageItem(Rect rect, string path, string name) {
            base.DrawPackageItem( rect, path, name );
        }
        protected override void DrawAssemblyItem(Rect rect, string path, string name) {
            base.DrawAssemblyItem( rect, path, name );
        }
        protected override void DrawAssetsItem(Rect rect, string path, string name, string rest) {
            base.DrawAssetsItem( rect, path, name, rest );
        }
        protected override void DrawResourcesItem(Rect rect, string path, string name, string rest) {
            base.DrawResourcesItem( rect, path, name, rest );
        }
        protected override void DrawSourcesItem(Rect rect, string path, string name, string rest) {
            base.DrawSourcesItem( rect, path, name, rest );
        }

    }
}
#endif

#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public class ProjectWindow2 : ProjectWindow {

        // Constructor
        static ProjectWindow2() {
            new ProjectWindow2();
        }

        // Constructor
        public ProjectWindow2() {
        }
        public override void Dispose() {
            base.Dispose();
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

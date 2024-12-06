# Overview
The package provides you with a more convenient project window that highlights the most important files and folders in special colors.
This highlights packages, assemblies, assets, resources and sources in the appropriate colors.
Thus, it makes working with the project window much more convenient and faster.

# How to use it
```
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
```

# Preferences
You can customize the colors in the ```Preferences/Colorful Project Window``` window.

# Media
- ![1](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e825a503-0649-474d-8f4e-2f770dc1fb5a)
- ![2](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/74f55fd5-39f9-4b1b-a662-71d52e02cae0)
- ![3](https://github.com/Denis535/CleanArchitectureGameFramework/assets/7755015/e3a77f0e-1c00-4382-b9df-bd3313dfc305)

# Links
- https://denis535.github.io
- https://www.nuget.org/profiles/Denis535
- https://openupm.com/packages/?sort=downloads&q=denis535
- https://www.fab.com/sellers/Denis535
- https://assetstore.unity.com/publishers/90787
- https://www.youtube.com/channel/UCLFdZl0pFkCkHpDWmodBUFg
- https://www.udemy.com/user/denis-84102

# If you want to support me
If you want to support me, please rate my packages, subscribe to my YouTube channel and like my videos.

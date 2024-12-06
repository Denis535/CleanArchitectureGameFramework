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

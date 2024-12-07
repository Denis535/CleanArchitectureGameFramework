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
            base.DrawElement( rect, path );
        }

        // DrawPackage
        protected override void DrawPackage(Rect rect, string path, string package) {
            base.DrawPackage( rect, path, package );
        }
        protected override void DrawAssembly(Rect rect, string path, string? package, string assembly) {
            base.DrawAssembly( rect, path, package, assembly );
        }
        protected override void DrawAssets(Rect rect, string path, string? package, string assembly, string content) {
            base.DrawAssets( rect, path, package, assembly, content );
        }
        protected override void DrawResources(Rect rect, string path, string? package, string assembly, string content) {
            base.DrawResources( rect, path, package, assembly, content );
        }
        protected override void DrawSources(Rect rect, string path, string? package, string assembly, string content) {
            base.DrawSources( rect, path, package, assembly, content );
        }

        // IsPackage
        protected override bool IsPackage(string path, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content) {
            if (IsMatch( path, "Packages/com.denis535.addressables-extensions", out package, out content )) {
                return true;
            }
            if (IsMatch( path, "Packages/com.denis535.addressables-source-generator", out package, out content )) {
                return true;
            }
            if (IsMatch( path, "Packages/com.denis535.clean-architecture-game-framework", out package, out content )) {
                return true;
            }
            if (IsMatch( path, "Packages/com.denis535.colorful-project-window", out package, out content )) {
                return true;
            }
            package = null;
            content = null;
            return false;
        }
        protected override bool IsAssembly(string path, string? package, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content) {
            if (IsMatch( path, "Assets/MyProject", out assembly, out content )) {
                return true;
            }
            if (IsMatch( path, "Assets/MyProject2", out assembly, out content )) {
                return true;
            }
            if (package != null) {
                if (IsMatch( path, "Packages/com.denis535.addressables-extensions/Runtime/Denis535.Addressables.Extensions", out assembly, out content )) {
                    return true;
                }
                if (IsMatch( path, "Packages/com.denis535.addressables-source-generator/Editor/Denis535.Addressables.SourceGenerator", out assembly, out content )) {
                    return true;
                }
                if (IsMatch( path, "Packages/com.denis535.clean-architecture-game-framework/Runtime/Denis535.CleanArchitectureGameFramework", out assembly, out content )) {
                    return true;
                }
                if (IsMatch( path, "Packages/com.denis535.clean-architecture-game-framework/Runtime/Denis535.CleanArchitectureGameFramework.Additions", out assembly, out content )) {
                    return true;
                }
                if (IsMatch( path, "Packages/com.denis535.clean-architecture-game-framework/Runtime/Denis535.CleanArchitectureGameFramework.Internal", out assembly, out content )) {
                    return true;
                }
                if (IsMatch( path, "Packages/com.denis535.clean-architecture-game-framework/Editor/Denis535.CleanArchitectureGameFramework.Editor", out assembly, out content )) {
                    return true;
                }
                if (IsMatch( path, "Packages/com.denis535.colorful-project-window/Editor/Denis535.ColorfulProjectWindow", out assembly, out content )) {
                    return true;
                }
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

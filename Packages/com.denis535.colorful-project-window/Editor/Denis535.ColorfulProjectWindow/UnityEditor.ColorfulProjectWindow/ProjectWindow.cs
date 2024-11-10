#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    public class ProjectWindow : ProjectWindowBase {

        // Settings
        protected Settings Settings => Settings.Instance;
        // PackagePaths
        protected string[] PackagePaths { get; }
        // AssemblyPaths
        protected string[] AssemblyPaths { get; }

        // Constructor
        public ProjectWindow() {
            PackagePaths = AssetDatabase.GetAllAssetPaths()
                .Where( i => Path.GetFileName( i ) is "package.json" )
                .Select( Path.GetDirectoryName )
                .Select( i => i.Replace( '\\', '/' ) )
                .OrderByDescending( i => i.StartsWith( "Assets/" ) )
                .Distinct()
                .ToArray();
            AssemblyPaths = AssetDatabase.GetAllAssetPaths()
                    .Where( i => Path.GetExtension( i ) is ".asmdef" or ".asmref" )
                    .Select( Path.GetDirectoryName )
                    .Select( i => i.Replace( '\\', '/' ) )
                    .OrderByDescending( i => i.StartsWith( "Assets/" ) )
                    .Distinct()
                    .ToArray();
        }
        public ProjectWindow(string[] packagePaths, string[] assemblyPaths) {
            PackagePaths = packagePaths;
            AssemblyPaths = assemblyPaths;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnGUI
        protected override void OnGUI(string guid, Rect rect) {
            base.OnGUI( guid, rect );
        }

        // DrawElement
        protected override void DrawPackageElement(Rect rect, string path, string name, string rest) {
            base.DrawPackageElement( rect, path, name, rest );
        }
        protected override void DrawAssemblyElement(Rect rect, string path, string name, string rest) {
            base.DrawAssemblyElement( rect, path, name, rest );
        }
        protected override void DrawAssemblyContentElement(Rect rect, string path, string name, string rest) {
            base.DrawAssemblyContentElement( rect, path, name, rest );
        }

        // DrawItem
        protected override void DrawPackageItem(Rect rect, string path, string name) {
            Highlight( rect, Settings.PackageColor, path, name );
        }
        protected override void DrawAssemblyItem(Rect rect, string path, string name) {
            Highlight( rect, Settings.AssemblyColor, path, name );
        }
        protected override void DrawAssetsItem(Rect rect, string path, string name, string rest) {
            Highlight( rect, Settings.AssetsColor, path, name, rest );
        }
        protected override void DrawResourcesItem(Rect rect, string path, string name, string rest) {
            Highlight( rect, Settings.ResourcesColor, path, name, rest );
        }
        protected override void DrawSourcesItem(Rect rect, string path, string name, string rest) {
            Highlight( rect, Settings.SourcesColor, path, name, rest );
        }

        // IsPackage
        protected override bool IsPackage(string path, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? rest) {
            // Assets/Packages/[Name]
            // Assets/Packages/[Name]/[Rest]
            var packagePath = PackagePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (packagePath != null) {
                name = Path.GetFileName( packagePath );
                rest = path.Substring( packagePath.Length ).TrimStart( '/' );
                return true;
            }
            name = null;
            rest = null;
            return false;
        }
        protected override bool IsAssembly(string path, [NotNullWhen( true )] out string? name, [NotNullWhen( true )] out string? rest) {
            // Assets/Assemblies/[Name]
            // Assets/Assemblies/[Name]/[Rest]
            var assemblyPath = AssemblyPaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (assemblyPath != null) {
                name = Path.GetFileName( assemblyPath );
                rest = path.Substring( assemblyPath.Length ).TrimStart( '/' );
                return true;
            }
            name = null;
            rest = null;
            return false;
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

        // Helpers
        protected static void Highlight(Rect rect, Color color, string path, string name) {
            if (rect.height == 16) {
                rect.x -= 16;
                rect.width = 16;
                rect.height = 16;
            } else {
                rect.width = 64;
                rect.height = 64;
            }
            DrawRect( rect, color );
        }
        protected static void Highlight(Rect rect, Color color, string path, string name, string rest) {
            if (rect.height == 16) {
                rect.x -= 16;
                rect.width = 16;
                rect.height = 16;
            } else {
                rect.width = 64;
                rect.height = 64;
            }
            if (rest.Contains( '/' )) {
                color = Darken( color, 1.5f );
            }
            DrawRect( rect, color );
        }
        // Helpers
        protected static Color HSVA(int h, float s, float v, float a) {
            var color = Color.HSVToRGB( h / 360f, s, v );
            color.a = a;
            return color;
        }
        protected static Color Lighten(Color color, float factor) {
            Color.RGBToHSV( color, out var h, out var s, out var v );
            var result = Color.HSVToRGB( h, s, v * factor );
            result.a = color.a;
            return result;
        }
        protected static Color Darken(Color color, float factor) {
            Color.RGBToHSV( color, out var h, out var s, out var v );
            var result = Color.HSVToRGB( h, s, v / factor );
            result.a = color.a;
            return result;
        }
        protected static void DrawRect(Rect rect, Color color) {
            var prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture( rect, Texture2D.whiteTexture );
            GUI.color = prev;
        }

    }
}

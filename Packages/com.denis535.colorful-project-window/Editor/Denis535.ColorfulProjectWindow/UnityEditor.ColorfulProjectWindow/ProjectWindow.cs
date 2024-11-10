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

        // DrawElement
        protected override void DrawElement(Rect rect, string path) {
            base.DrawElement( rect, path );
        }
        protected override void DrawPackage(Rect rect, string path, string package, string content) {
            if (content == string.Empty) {
                DrawItem( rect, Settings.PackageColor );
            }
        }
        protected override void DrawAssembly(Rect rect, string path, string assembly, string content) {
            if (content == string.Empty) {
                DrawItem( rect, Settings.AssemblyColor );
            } else {
                DrawAssemblyContent( rect, path, assembly, content );
            }
        }
        protected virtual void DrawAssemblyContent(Rect rect, string path, string assembly, string content) {
            if (IsFile( path ) && !content.Contains( '/' )) {
                return;
            }
            if (Path.GetExtension( path ) is ".asmdef" or ".asmref" or ".rsp") {
                return;
            }
            if (IsAssets( path, assembly, content )) {
                DrawAssemblyAssets( rect, path, assembly, content );
            } else
            if (IsResources( path, assembly, content )) {
                DrawAssemblyResources( rect, path, assembly, content );
            } else
            if (IsSources( path, assembly, content )) {
                DrawAssemblySources( rect, path, assembly, content );
            }
        }
        protected virtual void DrawAssemblyAssets(Rect rect, string path, string assembly, string content) {
            DrawItem( rect, Settings.AssetsColor, content.Contains( '/' ) );
        }
        protected virtual void DrawAssemblyResources(Rect rect, string path, string assembly, string content) {
            DrawItem( rect, Settings.ResourcesColor, content.Contains( '/' ) );
        }
        protected virtual void DrawAssemblySources(Rect rect, string path, string assembly, string content) {
            DrawItem( rect, Settings.SourcesColor, content.Contains( '/' ) );
        }

        // Helpers
        protected override bool IsPackage(string path, [NotNullWhen( true )] out string? package, [NotNullWhen( true )] out string? content) {
            // Assets/Packages/[Package]
            // Assets/Packages/[Package]/[Content]
            var packagePath = PackagePaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (packagePath != null) {
                package = Path.GetFileName( packagePath );
                content = path.Substring( packagePath.Length ).TrimStart( '/' );
                return true;
            }
            package = null;
            content = null;
            return false;
        }
        protected override bool IsAssembly(string path, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content) {
            // Assets/Assemblies/[Assembly]
            // Assets/Assemblies/[Assembly]/[Content]
            var assemblyPath = AssemblyPaths.FirstOrDefault( i => path.Equals( i ) || path.StartsWith( i + '/' ) );
            if (assemblyPath != null) {
                assembly = Path.GetFileName( assemblyPath );
                content = path.Substring( assemblyPath.Length ).TrimStart( '/' );
                return true;
            }
            assembly = null;
            content = null;
            return false;
        }
        protected virtual bool IsAssets(string path, string assembly, string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        protected virtual bool IsResources(string path, string assembly, string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        protected virtual bool IsSources(string path, string assembly, string content) {
            return true;
        }
        // Helpers
        protected static bool IsFile(string path) {
            return !AssetDatabase.IsValidFolder( path );
        }
        protected static bool IsFolder(string path) {
            return AssetDatabase.IsValidFolder( path );
        }
        // Helpers
        protected static void DrawItem(Rect rect, Color color) {
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
        protected static void DrawItem(Rect rect, Color color, bool isDarken) {
            if (rect.height == 16) {
                rect.x -= 16;
                rect.width = 16;
                rect.height = 16;
            } else {
                rect.width = 64;
                rect.height = 64;
            }
            if (isDarken) {
                color = Darken( color, 1.5f );
            }
            DrawRect( rect, color );
        }

    }
}

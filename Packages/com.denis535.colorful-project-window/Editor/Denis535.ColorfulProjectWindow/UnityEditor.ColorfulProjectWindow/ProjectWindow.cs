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
                DrawItem( rect, Settings.PackageColor, 0 );
            }
        }
        protected override void DrawAssembly(Rect rect, string path, string assembly, string content) {
            if (content == string.Empty) {
                DrawItem( rect, Settings.AssemblyColor, 0 );
            } else {
                if (AssetDatabase.IsValidFolder( path ) || content.Contains( '/' )) {
                    var depth = content.Count( i => i == '/' );
                    if (IsAssets( path, assembly, content )) {
                        DrawItem( rect, Settings.AssetsColor, depth );
                    } else if (IsResources( path, assembly, content )) {
                        DrawItem( rect, Settings.ResourcesColor, depth );
                    } else if (IsSources( path, assembly, content )) {
                        DrawItem( rect, Settings.SourcesColor, depth );
                    }
                }
            }
        }

        // IsPackage
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

    }
}

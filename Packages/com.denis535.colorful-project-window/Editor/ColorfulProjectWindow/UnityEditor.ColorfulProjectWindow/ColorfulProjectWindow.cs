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

    public class ColorfulProjectWindow : ColorfulProjectWindowBase {

        // AssemblyPaths
        protected string[] AssemblyPaths { get; }

        // Constructor
        public ColorfulProjectWindow() {
            AssemblyPaths = AssetDatabase.GetAllAssetPaths()
                    .Where( i => Path.GetExtension( i ) is ".asmdef" or ".asmref" )
                    .Select( Path.GetDirectoryName )
                    .Select( i => i.Replace( '\\', '/' ) )
                    .OrderByDescending( i => i.StartsWith( "Assets/" ) )
                    .Distinct()
                    .ToArray();
        }
        public ColorfulProjectWindow(string[] assemblyPaths) {
            AssemblyPaths = assemblyPaths;
        }

        // OnGUI
        protected override void OnGUI(Rect rect, string path) {
            base.OnGUI( rect, path );
        }

        // IsAssembly
        protected override bool IsAssembly(string path, [NotNullWhen( true )] out string? assembly, [NotNullWhen( true )] out string? content) {
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

        // DrawAssembly
        protected override void DrawAssembly(Rect rect, string path, string assembly, string content) {
            if (content == string.Empty) {
                DrawItem( rect, ColorfulProjectWindowSettings.Instance.AssemblyColor, 0 );
            } else {
                if (IsContent( path, assembly, content )) {
                    if (IsAssets( path, assembly, content )) {
                        DrawItem( rect, ColorfulProjectWindowSettings.Instance.AssetsColor, 0 );
                    } else if (IsResources( path, assembly, content )) {
                        DrawItem( rect, ColorfulProjectWindowSettings.Instance.ResourcesColor, 0 );
                    } else if (IsSources( path, assembly, content )) {
                        DrawItem( rect, ColorfulProjectWindowSettings.Instance.SourcesColor, 0 );
                    }
                }
            }
        }

        // Heleprs
        protected static bool IsContent(string path, string assembly, string content) {
            return AssetDatabase.IsValidFolder( path ) || content.Contains( '/' );
        }
        protected static bool IsAssets(string path, string assembly, string content) {
            return content.Equals( "Assets" ) || content.StartsWith( "Assets/" ) || content.StartsWith( "Assets." );
        }
        protected static bool IsResources(string path, string assembly, string content) {
            return content.Equals( "Resources" ) || content.StartsWith( "Resources/" ) || content.StartsWith( "Resources." );
        }
        protected static bool IsSources(string path, string assembly, string content) {
            return true;
        }

    }
}

#nullable enable
namespace CleanArchitectureGameFramework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using UnityEditor;
    using UnityEngine;

    public static class Toolbar {

        // OpenAll
        [MenuItem( "Tools/Clean Architecture Game Framework/Open All (UIAudioTheme)", priority = 0 )]
        public static void OpenAll_UIAudioTheme() {
            OpenAssetsReverse( "Assets/(*ThemeBase.cs|*Theme.cs)" );
        }
        [MenuItem( "Tools/Clean Architecture Game Framework/Open All (UIScreen)", priority = 1 )]
        public static void OpenAll_UIScreen() {
            OpenAssetsReverse( "Assets/(*ScreenBase.cs|*Screen.cs)" );
        }
        [MenuItem( "Tools/Clean Architecture Game Framework/Open All (UIWidget)", priority = 2 )]
        public static void OpenAll_UIWidget() {
            OpenAssetsReverse( "Assets/(*WidgetBase.cs|*Widget.cs)" );
        }
        [MenuItem( "Tools/Clean Architecture Game Framework/Open All (UIView)", priority = 3 )]
        public static void OpenAll_UIView() {
            OpenAssetsReverse( "Assets/(*ViewBase.cs|*View.cs)" );
        }

        // Helpers
        private static void OpenAssets(params string[] patterns) {
            foreach (var path in GetPaths().GetMatches( patterns )) {
                if (!Path.GetFileName( path ).StartsWith( "_" )) {
                    AssetDatabase.OpenAsset( AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( path ) );
                    Thread.Sleep( 100 );
                }
            }
        }
        private static void OpenAssetsReverse(params string[] patterns) {
            foreach (var path in GetPaths().GetMatches( patterns ).Reverse()) {
                if (!Path.GetFileName( path ).StartsWith( "_" )) {
                    AssetDatabase.OpenAsset( AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( path ) );
                    Thread.Sleep( 100 );
                }
            }
        }
        // Helpers
        private static string[] GetPaths() {
            var path = Directory.GetCurrentDirectory();
            return GetPaths( path )
                .Select( i => Path.GetRelativePath( path, i ) )
                .Select( i => i.Replace( '\\', '/' ) )
                .ToArray();
        }
        private static IEnumerable<string> GetPaths(string path) {
            var files = Directory.EnumerateFiles( path )
                .OrderBy( i => i );
            var directories = Directory.EnumerateDirectories( path )
                .OrderBy( i => !i.EndsWith( ".UI" ) )
                .ThenBy( i => !i.EndsWith( ".UI.MainScreen" ) )
                .ThenBy( i => !i.EndsWith( ".UI.GameScreen" ) )
                .ThenBy( i => !i.EndsWith( ".UI.Common" ) )
                .ThenBy( i => !i.EndsWith( ".App" ) )
                .ThenBy( i => !i.EndsWith( ".Entities" ) )
                .ThenBy( i => !i.EndsWith( ".Entities.MainScene" ) )
                .ThenBy( i => !i.EndsWith( ".Entities.GameScene" ) )
                .ThenBy( i => !i.EndsWith( ".Entities.WorldScene" ) )
                .ThenBy( i => !i.EndsWith( ".Entities.Common" ) )
                .ThenBy( i => !i.EndsWith( ".Common" ) )
                .ThenBy( i => i );
            foreach (var file in files) {
                yield return file;
            }
            foreach (var dir in directories) {
                yield return dir;
                foreach (var i in GetPaths( dir )) yield return i;
            }
        }
        // Helpers
        private static IEnumerable<string> GetMatches(this string[] paths, string[] patterns) {
            foreach (var pattern in patterns) {
                var regex = new Regex( "^" + pattern.Replace( " ", "" ).Replace( "*", "(.*?)" ) + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace );
                foreach (var path in paths.Where( path => regex.IsMatch( path ) )) {
                    yield return path;
                }
            }
        }

    }
}

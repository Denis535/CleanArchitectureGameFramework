#nullable enable
namespace UnityEditor {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    public static class MenuBar {

        // TakeScreenshot
        [MenuItem( "Tools/Clean Architecture Game Framework/Take Screenshot (Game) _F12", priority = 0 )]
        internal static void TakeScreenshot_Game() {
            var path = $"Screenshots/{Application.productName}-{DateTime.UtcNow.Ticks}.png";
            ScreenCapture.CaptureScreenshot( path, 1 );
            EditorApplication.Beep();
            EditorUtility.RevealInFinder( path );
        }
        [MenuItem( "Tools/Clean Architecture Game Framework/Take Screenshot (Editor) &F12", priority = 1 )]
        internal static void TakeScreenshot_Editor() {
            var position = EditorGUIUtility.GetMainWindowPosition();
            var texture = new Texture2D( (int) position.width, (int) position.height );
            texture.SetPixels( InternalEditorUtility.ReadScreenPixel( position.position, (int) position.width, (int) position.height ) );
            var bytes = texture.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate( texture );

            var path = $"Screenshots/{Application.productName}-{DateTime.UtcNow.Ticks}.png";
            Directory.CreateDirectory( Path.GetDirectoryName( path ) );
            File.WriteAllBytes( path, bytes );
            EditorApplication.Beep();
            EditorUtility.RevealInFinder( path );
        }

        //// OpenAll
        //[MenuItem( "Tools/Clean Architecture Game Framework/Open All", priority = 100 )]
        //public static void OpenAll_CSharp() {
        //    OpenAssetsReverse( "Assets/(*.cs)" );
        //}
        //[MenuItem( "Tools/Clean Architecture Game Framework/Open All (UI)", priority = 101 )]
        //public static void OpenAll_UI() {
        //    OpenAssetsReverse( "Assets/(*/UI/|*/UI.*/)(*.cs)" );
        //}

        //// Helpers
        //private static void OpenAssets(params string[] patterns) {
        //    foreach (var path in GetPaths().GetMatches( patterns )) {
        //        AssetDatabase.OpenAsset( AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( path ) );
        //        Thread.Sleep( 100 );
        //    }
        //}
        //private static void OpenAssetsReverse(params string[] patterns) {
        //    foreach (var path in GetPaths().GetMatches( patterns ).Reverse()) {
        //        AssetDatabase.OpenAsset( AssetDatabase.LoadAssetAtPath<UnityEngine.Object>( path ) );
        //        Thread.Sleep( 100 );
        //    }
        //}
        //// Helpers
        //private static string[] GetPaths() {
        //    var path = Directory.GetCurrentDirectory();
        //    return GetPaths( path )
        //        .Select( i => Path.GetRelativePath( path, i ) )
        //        .Select( i => i.Replace( '\\', '/' ) )
        //        .ToArray();
        //}
        //private static IEnumerable<string> GetPaths(string path) {
        //    var files = Directory.EnumerateFiles( path )
        //        .OrderBy( i => i );
        //    var directories = Directory.EnumerateDirectories( path )
        //        .OrderBy( i => !i.EndsWith( ".UI" ) )
        //        .ThenBy( i => !i.EndsWith( ".App" ) )
        //        .ThenBy( i => !i.EndsWith( ".Domain" ) )
        //        .ThenBy( i => !i.EndsWith( ".Entities" ) )
        //        .ThenBy( i => !i.EndsWith( ".MainScreen" ) )
        //        .ThenBy( i => !i.EndsWith( ".GameScreen" ) )
        //        .ThenBy( i => !i.EndsWith( ".Actors" ) )
        //        .ThenBy( i => !i.EndsWith( ".Things" ) )
        //        .ThenBy( i => !i.EndsWith( ".Vehicles" ) )
        //        .ThenBy( i => !i.EndsWith( ".Transports" ) )
        //        .ThenBy( i => !i.EndsWith( ".Worlds" ) )
        //        .ThenBy( i => !i.EndsWith( ".Common" ) )
        //        .ThenBy( i => i );
        //    foreach (var file in files) {
        //        yield return file;
        //    }
        //    foreach (var dir in directories) {
        //        yield return dir;
        //        foreach (var i in GetPaths( dir )) yield return i;
        //    }
        //}
        //// Helpers
        //private static IEnumerable<string> GetMatches(this string[] paths, string[] patterns) {
        //    foreach (var pattern in patterns) {
        //        var regex = new Regex( "^" + pattern.Replace( " ", "" ).Replace( "*", "(.*?)" ) + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace );
        //        foreach (var path in paths.Where( path => regex.IsMatch( path ) )) {
        //            yield return path;
        //        }
        //    }
        //}

    }
}

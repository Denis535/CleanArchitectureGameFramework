#nullable enable
namespace CleanArchitectureGameFramework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    public static class PackageMenuBar {

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

        // AboutPackage
        [MenuItem( "Tools/Clean Architecture Game Framework/About Package", priority = 1_000_000 )]
        public static void AboutPackage() {
            _ = EditorWindow.GetWindow<AboutPackageWindow>();
        }

    }
}

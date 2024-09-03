#nullable enable
namespace UnityEditor {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Framework;

    public class AboutWindow : EditorWindow {

        // Show
        [MenuItem( "Tools/Clean Architecture Game Framework/About Clean Architecture Game Framework", priority = 10000 )]
        public new static void Show() {
            var window = GetWindow<AboutWindow>( true, "About Clean Architecture Game Framework", true );
            window.minSize = window.maxSize = new Vector2( 800, 600 );
        }

        // OnGUI
        public void OnGUI() {
            HelpBox.Draw();
        }

    }
}

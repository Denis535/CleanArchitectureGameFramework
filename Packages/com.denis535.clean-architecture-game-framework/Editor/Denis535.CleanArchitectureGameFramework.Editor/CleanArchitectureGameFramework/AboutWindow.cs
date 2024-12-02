#nullable enable
namespace UnityEditor {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Framework;

    public class AboutWindow : EditorWindow {

        // Constructor
        public AboutWindow() {
            titleContent = new GUIContent( "About Clean Architecture Game Framework package" );
            minSize = maxSize = new Vector2( 1200, 800 );
        }

        // OnEnable
        public void OnEnable() {
            ShowUtility();
            Focus();
        }
        public void OnDisable() {
        }

        // OnGUI
        public void OnGUI() {
            HelpBox.Draw();
        }

    }
}

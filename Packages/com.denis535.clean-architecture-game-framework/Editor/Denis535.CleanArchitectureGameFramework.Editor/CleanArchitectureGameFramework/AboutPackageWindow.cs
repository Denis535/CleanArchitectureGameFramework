#nullable enable
namespace CleanArchitectureGameFramework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Framework;

    public class AboutPackageWindow : EditorWindow {

        // Constructor
        public AboutPackageWindow() {
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

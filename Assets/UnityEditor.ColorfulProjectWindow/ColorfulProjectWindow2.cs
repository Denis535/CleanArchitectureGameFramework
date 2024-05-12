#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.ColorfulProjectWindow {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public class ColorfulProjectWindow2 : ColorfulProjectWindow {

        // Constructor
        static ColorfulProjectWindow2() {
            new ColorfulProjectWindow2();
        }

        // Constructor
        public ColorfulProjectWindow2() {
        }

        // OnGUI
        protected override void OnGUI(string guid, Rect rect) {
            base.OnGUI( guid, rect );
        }

    }
}
#endif

#if UNITY_EDITOR
#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor( typeof( UIAudioThemeBase ), true )]
    public class UIAudioThemeEditor : Editor {

        protected UIAudioThemeBase Target => (UIAudioThemeBase) target;

        // OnInspectorGUI
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
        public override bool RequiresConstantRepaint() {
            return EditorApplication.isPlaying;
        }

    }
}
#endif

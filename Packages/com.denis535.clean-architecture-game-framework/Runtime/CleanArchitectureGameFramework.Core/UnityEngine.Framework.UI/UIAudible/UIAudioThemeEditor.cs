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

        // Target
        protected UIAudioThemeBase Target => (UIAudioThemeBase) target;

        // Awake
        public virtual void Awake() {
        }
        public virtual void OnDestroy() {
        }

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

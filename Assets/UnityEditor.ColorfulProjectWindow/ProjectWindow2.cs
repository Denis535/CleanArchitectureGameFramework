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
    public class ProjectWindow2 : ProjectWindow {

        // Constructor
        static ProjectWindow2() {
            new ProjectWindow2();
        }

        // Constructor
        public ProjectWindow2() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
}
#endif

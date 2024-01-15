#if UNITY_EDITOR
#nullable enable
namespace UnityEditor.Tools_ {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class ProjectBuilder {

        // Build/Pre
        public virtual void PreBuild() {
        }

        // Build/Development
        public virtual void BuildDevelopment(string path) {
        }

        // Build/Production
        public virtual void BuildProduction(string path) {
        }

    }
}
#endif

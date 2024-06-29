#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [DefaultExecutionOrder( 1000 )]
    public abstract class ProgramBase : MonoBehaviour {

        // Awake
        protected virtual void Awake() {
            Application.wantsToQuit += OnQuit;
        }
        protected virtual void OnDestroy() {
        }

#if UNITY_EDITOR
        // OnInspectorGUI
        protected internal virtual void OnInspectorGUI() {
        }
#endif

        // Start
        protected virtual void Start() {
        }
        protected virtual void FixedUpdate() {
        }
        protected virtual void Update() {
        }
        protected virtual void LateUpdate() {
        }

        // OnQuit
        protected virtual bool OnQuit() {
            return true;
        }

    }
}

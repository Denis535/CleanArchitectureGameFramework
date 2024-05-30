#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ProgramBase : MonoBehaviour {

        // Awake
        protected abstract void Awake();
        protected abstract void OnDestroy();

    }
}

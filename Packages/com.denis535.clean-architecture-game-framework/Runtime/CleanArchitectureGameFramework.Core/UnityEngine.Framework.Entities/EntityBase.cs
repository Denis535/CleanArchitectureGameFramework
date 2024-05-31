#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class EntityBase : MonoBehaviour {

        // Awake
        protected abstract void Awake();
        protected abstract void OnDestroy();

    }
}

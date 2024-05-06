#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class LevelViewBase : IDisposable {

        // GameObject
        protected abstract GameObject GameObject { get; }
        // Transform
        protected Transform Transform => GameObject.transform;

        // Constructor
        public LevelViewBase() {
        }
        public abstract void Dispose();

    }
}

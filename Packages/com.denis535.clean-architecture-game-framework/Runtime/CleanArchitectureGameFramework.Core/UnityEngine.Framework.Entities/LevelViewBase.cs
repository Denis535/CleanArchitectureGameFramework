#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class LevelViewBase : IDisposable {

        // GameObject
        public abstract GameObject GameObject { get; }
        // Transform
        public Transform Transform => GameObject.transform;

        // Constructor
        public LevelViewBase() {
        }
        public abstract void Dispose();

    }
}

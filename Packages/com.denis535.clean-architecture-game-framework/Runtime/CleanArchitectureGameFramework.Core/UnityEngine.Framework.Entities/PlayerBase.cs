#nullable enable
namespace UnityEngine.Framework.Entities {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PlayerBase : IDisposable {

        // Constructor
        public PlayerBase() { }
        public abstract void Dispose();

    }
}

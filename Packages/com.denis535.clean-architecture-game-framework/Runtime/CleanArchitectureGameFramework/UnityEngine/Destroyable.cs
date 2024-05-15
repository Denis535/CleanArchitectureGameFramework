#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Destroyable : MonoBehaviour {

        public event Action? OnDestroyEvent;

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
            OnDestroyEvent?.Invoke();
        }

    }
    public static class DestroyableExtensions {

        public static void OnDestroy(this GameObject gameObject, Action callback) {
            var destroyable = gameObject.GetComponent<Destroyable>() ?? gameObject.AddComponent<Destroyable>();
            destroyable.OnDestroyEvent += callback;
        }

    }
}

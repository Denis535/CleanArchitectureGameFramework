#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class Destroyable : MonoBehaviour {

        internal AsyncOperationHandle<GameObject> PrefabHandle { get; set; }

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
            Addressables.Release( PrefabHandle );
        }

    }
    public static class DestroyableExtensions {

        public static void AddDestroyable(this GameObject gameObject, AsyncOperationHandle<GameObject> prefabHandle) {
            var destroyable = gameObject.AddComponent<Destroyable>();
            destroyable.PrefabHandle = prefabHandle;
        }

    }
}

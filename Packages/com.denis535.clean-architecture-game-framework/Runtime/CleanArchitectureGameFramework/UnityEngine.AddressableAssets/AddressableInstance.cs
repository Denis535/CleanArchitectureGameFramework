#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AddressableInstance : MonoBehaviour {

        public AsyncOperationHandle<GameObject> PrefabHandle { get; internal set; }

        // Awake
        public void Awake() {
        }
        public void OnDestroy() {
            Addressables.Release( PrefabHandle );
        }

    }
    public static class AddressableInstanceExtensions {

        public static AddressableInstance AddAddressableInstance(this GameObject instance, AsyncOperationHandle<GameObject> prefabHandle) {
            var component = instance.AddComponent<AddressableInstance>();
            component.PrefabHandle = prefabHandle;
            return component;
        }

    }
}

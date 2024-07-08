#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneHandleDynamic : AddressableHandleDynamic<SceneHandle>, ISceneHandle<SceneHandleDynamic> {

        // Constructor
        public SceneHandleDynamic() {
        }

        // SetUp
        public new SceneHandleDynamic SetUp(SceneHandle? handle) {
            return (SceneHandleDynamic) base.SetUp( handle );
        }

        // Load
        public SceneHandleDynamic Load(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_HasHandle();
            Handle.Load( loadMode, activateOnLoad );
            return this;
        }

        // Wait
        public ValueTask WaitAsync() {
            Assert_HasHandle();
            return Handle.WaitAsync();
        }

        // GetValue
        public ValueTask<Scene> GetValueAsync() {
            Assert_HasHandle();
            return Handle.GetValueAsync();
        }

        // Activate
        public ValueTask<Scene> ActivateAsync() {
            Assert_HasHandle();
            return Handle.ActivateAsync();
        }

        // Unload
        public void Unload() {
            Assert_HasHandle();
            Handle.Unload();
        }
        public async ValueTask UnloadAsync() {
            Assert_HasHandle();
            await Handle.UnloadAsync();
        }

        // UnloadSafe
        public void UnloadSafe() {
            if (Handle != null && Handle.IsValid) {
                Unload();
            }
        }
        public async ValueTask UnloadSafeAsync() {
            if (Handle != null && Handle.IsValid) {
                await UnloadAsync();
            }
        }

        // Utils
        public override string ToString() {
            if (Handle != null) {
                return "SceneHandleDynamic: " + Handle.Key;
            } else {
                return "SceneHandleDynamic";
            }
        }

    }
}

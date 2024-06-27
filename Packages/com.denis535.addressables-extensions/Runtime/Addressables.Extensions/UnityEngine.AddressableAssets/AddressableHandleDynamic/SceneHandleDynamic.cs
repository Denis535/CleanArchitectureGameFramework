#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneHandleDynamic : AddressableHandleDynamic, ISceneHandle<SceneHandleDynamic> {

        // Handle
        [MemberNotNullWhen( true, "Handle" )]
        public override bool IsValid {
            get {
                return Handle != null;
            }
        }
        public SceneHandle? Handle { get; private set; }
        public override string Key {
            get {
                Assert_IsValid();
                return Handle!.Key;
            }
        }
        public override bool IsDone {
            get {
                Assert_IsValid();
                return Handle!.IsDone;
            }
        }
        public override bool IsSucceeded {
            get {
                Assert_IsValid();
                return Handle!.IsSucceeded;
            }
        }
        public override bool IsFailed {
            get {
                Assert_IsValid();
                return Handle!.IsFailed;
            }
        }

        // Constructor
        public SceneHandleDynamic() {
        }

        // SetUp
        public SceneHandleDynamic SetUp(string key) {
            Assert_IsNotValid();
            Handle = new SceneHandle( key );
            return this;
        }
        public SceneHandleDynamic SetUp(SceneHandle handle) {
            Assert_IsNotValid();
            Handle = handle;
            return this;
        }

        // Load
        public SceneHandleDynamic Load(LoadSceneMode loadMode, bool activateOnLoad) {
            Assert_IsValid();
            Handle!.Load( loadMode, activateOnLoad );
            return this;
        }

        // Wait
        public ValueTask WaitAsync() {
            Assert_IsValid();
            return Handle!.WaitAsync();
        }

        // GetValue
        public ValueTask<Scene> GetValueAsync() {
            Assert_IsValid();
            return Handle!.GetValueAsync();
        }

        // Activate
        public ValueTask<Scene> ActivateAsync() {
            Assert_IsValid();
            return Handle!.ActivateAsync();
        }

        // Unload
        public void Unload() {
            Assert_IsValid();
            Handle!.Unload();
            Handle = null;
        }
        public async ValueTask UnloadAsync() {
            Assert_IsValid();
            await Handle!.UnloadAsync();
            Handle = null;
        }

        // UnloadSafe
        public void UnloadSafe() {
            if (IsValid) {
                Unload();
            }
        }
        public async ValueTask UnloadSafeAsync() {
            if (IsValid) {
                await UnloadAsync();
            }
        }

        // Utils
        public override string ToString() {
            if (IsValid) {
                return "SceneHandleDynamic: " + Key;
            } else {
                return "SceneHandleDynamic";
            }
        }

    }
}

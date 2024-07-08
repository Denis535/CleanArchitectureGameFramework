#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PrefabListHandleDynamic<T> : AddressableListHandleDynamic<PrefabListHandle<T>>, IPrefabListHandle<PrefabListHandleDynamic<T>, T> where T : notnull, UnityEngine.Object {

        // Constructor
        public PrefabListHandleDynamic() {
        }

        // SetUp
        public new PrefabListHandleDynamic<T> SetUp(PrefabListHandle<T>? handle) {
            return (PrefabListHandleDynamic<T>) base.SetUp( handle );
        }

        // Load
        public PrefabListHandleDynamic<T> Load() {
            Assert_HasHandle();
            Handle.Load();
            return this;
        }

        // Wait
        public void Wait() {
            Assert_HasHandle();
            Handle.Wait();
        }
        public ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_HasHandle();
            return Handle.WaitAsync( cancellationToken );
        }

        // GetValue
        public IReadOnlyList<T> GetValues() {
            Assert_HasHandle();
            return Handle.GetValues();
        }
        public ValueTask<IReadOnlyList<T>> GetValuesAsync(CancellationToken cancellationToken) {
            Assert_HasHandle();
            return Handle.GetValuesAsync( cancellationToken );
        }

        // Release
        public void Release() {
            Assert_HasHandle();
            Handle.Release();
        }
        public void ReleaseSafe() {
            if (Handle != null && Handle.IsValid) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            if (Handle != null) {
                return "PrefabListHandleDynamic: " + string.Join( ", ", Handle.Keys );
            } else {
                return "PrefabListHandleDynamic";
            }
        }

    }
}

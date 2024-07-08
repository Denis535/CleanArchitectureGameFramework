#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    public class PrefabHandleDynamic<T> : AddressableHandleDynamic<PrefabHandle<T>>, IPrefabHandle<PrefabHandleDynamic<T>, T> where T : notnull, UnityEngine.Object {

        // Constructor
        public PrefabHandleDynamic() {
        }

        // SetHandle
        public PrefabHandleDynamic<T> SetHandle(string? key) {
            if (key != null) {
                return (PrefabHandleDynamic<T>) base.SetHandle( new PrefabHandle<T>( key ) );
            } else {
                return (PrefabHandleDynamic<T>) base.SetHandle( null );
            }
        }
        public new PrefabHandleDynamic<T> SetHandle(PrefabHandle<T>? handle) {
            return (PrefabHandleDynamic<T>) base.SetHandle( handle );
        }

        // Load
        public PrefabHandleDynamic<T> Load() {
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
        public T GetValue() {
            Assert_HasHandle();
            return Handle.GetValue();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_HasHandle();
            return Handle.GetValueAsync( cancellationToken );
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
                return "PrefabHandleDynamic: " + Handle.Key;
            } else {
                return "PrefabHandleDynamic";
            }
        }

    }
}

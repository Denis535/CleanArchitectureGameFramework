#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class InstanceHandle<T> : AddressableInstanceHandle<T>, ICloneable where T : notnull, Component {

        private string Key { get; }

        public InstanceHandle(string key) {
            Key = key;
        }
        private InstanceHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
            Addressables.ResourceManager.Acquire( Handle );
        }

        // InstantiateAsync
        public Task<T> InstantiateAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }
        public Task<T> InstantiateAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, position, rotation );
            return Handle.GetResultAsync( cancellationToken );
        }
        public Task<T> InstantiateAsync(Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public Task<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, position, rotation, parent );
            return Handle.GetResultAsync( cancellationToken );
        }

        // Utils
        object ICloneable.Clone() {
            return Clone();
        }
        public InstanceHandle<T> Clone() {
            Assert_IsValid();
            return new InstanceHandle<T>( Key, Handle );
        }

    }
    public class DynamicInstanceHandle<T> : AddressableInstanceHandle<T>, ICloneable where T : notnull, Component {

        private string? key;

        [AllowNull]
        public string Key {
            get {
                Assert_IsValid();
                return key!;
            }
            protected set {
                key = value;
            }
        }

        public DynamicInstanceHandle() {
        }
        private DynamicInstanceHandle(string key, AsyncOperationHandle<T> handle) {
            Key = key;
            Handle = handle;
            Addressables.ResourceManager.Acquire( Handle );
        }

        // InstantiateAsync
        public Task<T> InstantiateAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }
        public Task<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, position, rotation );
            return Handle.GetResultAsync( cancellationToken );
        }
        public Task<T> InstantiateAsync(string key, Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public Task<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, position, rotation, parent );
            return Handle.GetResultAsync( cancellationToken );
        }

        // Utils
        object ICloneable.Clone() {
            return Clone();
        }
        public DynamicInstanceHandle<T> Clone() {
            Assert_IsValid();
            return new DynamicInstanceHandle<T>( Key, Handle );
        }

    }
}

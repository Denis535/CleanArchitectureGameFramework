#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public interface IInstanceHandle<out T> where T : notnull, Component {

        // State
        bool IsValid { get; }
        bool IsDone { get; }
        bool IsSucceeded { get; }
        bool IsFailed { get; }
        // Value
        T Value { get; }
        T? ValueSafe { get; }
        // Exception
        Exception? Exception { get; }

        // InstantiateAsync
        //ValueTask<T> InstantiateAsync(CancellationToken cancellationToken);
        //ValueTask<T> InstantiateAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken);
        //ValueTask<T> InstantiateAsync(Transform parent, CancellationToken cancellationToken);
        //ValueTask<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent, CancellationToken cancellationToken);

        // GetValueAsync
        //ValueTask<T> GetValueAsync(CancellationToken cancellationToken);

        // ReleaseInstance
        void ReleaseInstance();
        void ReleaseInstanceSafe();

    }
    public class InstanceHandle<T> : AddressableHandle<T>, IInstanceHandle<T>, ICloneable where T : notnull, Component {

        // Constructor
        public InstanceHandle(string key) : base( key ) {
        }
        public InstanceHandle(string key, AsyncOperationHandle<T> handle) : base( key, handle ) {
        }

        // InstantiateAsync
        public ValueTask<T> InstantiateAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, position, rotation );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, position, rotation, parent );
            return Handle.GetResultAsync( cancellationToken );
        }

        // ReleaseInstance
        public void ReleaseInstance() {
            Assert_IsValid();
            Addressables.ReleaseInstance( Handle );
            Handle = default;
        }
        public void ReleaseInstanceSafe() {
            if (Handle.IsValid()) {
                ReleaseInstance();
            }
        }

        // Utils
        object ICloneable.Clone() {
            return Clone();
        }
        public InstanceHandle<T> Clone() {
            Assert_IsValid();
            var clone = new InstanceHandle<T>( Key, Handle );
            Addressables.ResourceManager.Acquire( Handle );
            return clone;
        }

    }
    public class DynamicInstanceHandle<T> : DynamicAddressableHandle<T>, IInstanceHandle<T>, ICloneable where T : notnull, Component {

        // Constructor
        public DynamicInstanceHandle() {
        }
        public DynamicInstanceHandle(string key, AsyncOperationHandle<T> handle) : base( key, handle ) {
        }

        // InstantiateAsync
        public ValueTask<T> InstantiateAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, position, rotation );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, position, rotation, parent );
            return Handle.GetResultAsync( cancellationToken );
        }

        // ReleaseInstance
        public void ReleaseInstance() {
            Assert_IsValid();
            Addressables.ReleaseInstance( Handle );
            Handle = default;
        }
        public void ReleaseInstanceSafe() {
            if (Handle.IsValid()) {
                ReleaseInstance();
            }
        }

        // Utils
        object ICloneable.Clone() {
            return Clone();
        }
        public InstanceHandle<T> Clone() {
            Assert_IsValid();
            var clone = new InstanceHandle<T>( Key, Handle );
            Addressables.ResourceManager.Acquire( Handle );
            return clone;
        }

    }
}

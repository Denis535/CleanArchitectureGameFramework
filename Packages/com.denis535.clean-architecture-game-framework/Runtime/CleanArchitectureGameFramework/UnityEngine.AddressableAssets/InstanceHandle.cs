#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class InstanceHandle : AddressableHandle {

        // Constructor
        public InstanceHandle(string key) : base( key ) {
        }

        // Release
        public abstract void Release();
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

    }
    public abstract class DynamicInstanceHandle : DynamicAddressableHandle {

        // Constructor
        public DynamicInstanceHandle(string? key) : base( key ) {
        }

        // Release
        public abstract void Release();
        public void ReleaseSafe() {
            if (IsValid) {
                Release();
            }
        }

    }
    public class InstanceHandle<T> : InstanceHandle, ICloneable where T : notnull, Component {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public T Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public T? ValueSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }

        // Constructor
        public InstanceHandle(string key) : base( key ) {
        }
        public InstanceHandle(string key, AsyncOperationHandle<T> handle) : base( key ) {
            Handle = handle;
        }

        // Instantiate
        public T Instantiate() {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key );
            return Handle.GetResult();
        }
        public T Instantiate(Vector3 position, Quaternion rotation) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, position, rotation );
            return Handle.GetResult();
        }
        public T Instantiate(Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, parent );
            return Handle.GetResult();
        }
        public T Instantiate(Vector3 position, Quaternion rotation, Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, position, rotation, parent );
            return Handle.GetResult();
        }
        public T Instantiate(Func<GameObject, T> instanceProvider) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, instanceProvider );
            return Handle.GetResult();
        }

        // InstantiateAsync
        public ValueTask<T> InstantiateAsync(CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, position, rotation );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Transform? parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, position, rotation, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(Func<GameObject, T> instanceProvider, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key, instanceProvider );
            return Handle.GetResultAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public override void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }

        // Utils
        object ICloneable.Clone() {
            return Clone();
        }
        public InstanceHandle<T> Clone() {
            if (Handle.IsValid()) Addressables.ResourceManager.Acquire( Handle );
            return new InstanceHandle<T>( Key, Handle );
        }

    }
    public class DynamicInstanceHandle<T> : DynamicInstanceHandle, ICloneable where T : notnull, Component {

        // Handle
        protected AsyncOperationHandle<T> Handle { get; set; }
        public override bool IsValid => Handle.IsValid();
        public override bool IsDone => Handle.IsDone;
        public override bool IsSucceeded => Handle.Status == AsyncOperationStatus.Succeeded;
        public override bool IsFailed => Handle.Status == AsyncOperationStatus.Failed;
        public override Exception? Exception => Handle.OperationException;
        public T Value {
            get {
                Assert_IsValid();
                Assert_IsSucceeded();
                return Handle.Result;
            }
        }
        public T? ValueSafe {
            get {
                return Handle.IsValid() && Handle.IsSucceeded() ? Handle.Result : default;
            }
        }

        // Constructor
        public DynamicInstanceHandle() : base( null ) {
        }
        public DynamicInstanceHandle(string? key, AsyncOperationHandle<T> handle) : base( key ) {
            Handle = handle;
        }

        // Instantiate
        public T Instantiate(string key) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key );
            return Handle.GetResult();
        }
        public T Instantiate(string key, Vector3 position, Quaternion rotation) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, position, rotation );
            return Handle.GetResult();
        }
        public T Instantiate(string key, Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, parent );
            return Handle.GetResult();
        }
        public T Instantiate(string key, Vector3 position, Quaternion rotation, Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, position, rotation, parent );
            return Handle.GetResult();
        }
        public T Instantiate(string key, Func<GameObject, T> instanceProvider) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, instanceProvider );
            return Handle.GetResult();
        }

        // InstantiateAsync
        public ValueTask<T> InstantiateAsync(string key, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, position, rotation );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Transform? parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, position, rotation, parent );
            return Handle.GetResultAsync( cancellationToken );
        }
        public ValueTask<T> InstantiateAsync(string key, Func<GameObject, T> instanceProvider, CancellationToken cancellationToken) {
            Assert_IsNotValid();
            Handle = AddressableHelper.InstantiateAsync<T>( Key = key, instanceProvider );
            return Handle.GetResultAsync( cancellationToken );
        }

        // GetValue
        public T GetValue() {
            Assert_IsValid();
            return Handle.GetResult();
        }
        public ValueTask<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.GetResultAsync( cancellationToken );
        }

        // Release
        public override void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Key = null;
            Handle = default;
        }

        // Utils
        object ICloneable.Clone() {
            return Clone();
        }
        public DynamicInstanceHandle<T> Clone() {
            if (Handle.IsValid()) Addressables.ResourceManager.Acquire( Handle );
            return new DynamicInstanceHandle<T>( Key, Handle );
        }

    }
}

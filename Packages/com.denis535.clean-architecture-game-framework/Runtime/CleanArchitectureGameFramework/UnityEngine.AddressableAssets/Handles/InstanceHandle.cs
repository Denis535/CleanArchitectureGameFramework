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

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

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

        // Wait
        public abstract void Wait();
        public abstract ValueTask WaitAsync(CancellationToken cancellationToken);

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
        public InstanceHandle<T> Instantiate() {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key );
            return this;
        }
        public InstanceHandle<T> Instantiate(Vector3 position, Quaternion rotation) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, position, rotation );
            return this;
        }
        public InstanceHandle<T> Instantiate(Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, parent );
            return this;
        }
        public InstanceHandle<T> Instantiate(Vector3 position, Quaternion rotation, Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, position, rotation, parent );
            return this;
        }
        public InstanceHandle<T> Instantiate(Func<GameObject, T> instanceProvider) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key, instanceProvider );
            return this;
        }

        // Wait
        public override void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public override ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.WaitAsync( cancellationToken );
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
        public DynamicInstanceHandle<T> Instantiate(string key) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key );
            return this;
        }
        public DynamicInstanceHandle<T> Instantiate(string key, Vector3 position, Quaternion rotation) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, position, rotation );
            return this;
        }
        public DynamicInstanceHandle<T> Instantiate(string key, Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, parent );
            return this;
        }
        public DynamicInstanceHandle<T> Instantiate(string key, Vector3 position, Quaternion rotation, Transform? parent) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, position, rotation, parent );
            return this;
        }
        public DynamicInstanceHandle<T> Instantiate(string key, Func<GameObject, T> instanceProvider) {
            Assert_IsNotValid();
            Handle = AddressableHandleHelper.InstantiateAsync<T>( Key = key, instanceProvider );
            return this;
        }

        // Wait
        public override void Wait() {
            Assert_IsValid();
            Handle.Wait();
        }
        public override ValueTask WaitAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            return Handle.WaitAsync( cancellationToken );
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

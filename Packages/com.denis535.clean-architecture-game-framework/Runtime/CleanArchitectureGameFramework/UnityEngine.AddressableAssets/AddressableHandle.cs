#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public abstract class AddressableHandle {

        // State
        public abstract bool IsValid { get; }
        public abstract bool IsSucceeded { get; }
        public abstract bool IsFailed { get; }
        // ValueObject
        public abstract object ValueObject { get; }
        public abstract object? ValueObjectSafe { get; }
        // Exception
        public abstract Exception? Exception { get; }

        // Constructor
        public AddressableHandle() {
        }

        // GetValueObjectAsync
        public abstract Task<object> GetValueObjectAsync(CancellationToken cancellationToken);

        // Utils
        public override string ToString() {
            return base.ToString();
        }

    }
    public abstract class AddressableHandle<T> : AddressableHandle where T : notnull {

        // Handle
        protected internal AsyncOperationHandle<T> Handle { get; set; }
        // State
        public override bool IsValid => Handle.IsValid();
        public override bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
        public override bool IsFailed => Handle.IsValid() && Handle.IsFailed();
        // Value
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
        // ValueObject
        public override object ValueObject => Value;
        public override object? ValueObjectSafe => ValueSafe;
        // Exception
        public override Exception? Exception => Handle.OperationException;

        // Constructor
        public AddressableHandle() {
        }

        // GetValueAsync
        public async Task<T> GetValueAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            return value;
        }

        // GetValueObjectAsync
        public override async Task<object> GetValueObjectAsync(CancellationToken cancellationToken) {
            var value = await GetValueAsync( cancellationToken );
            return value;
        }

        // Utils
        public override string ToString() {
            return Handle.DebugName;
        }

        // Heleprs
        protected internal void Assert_IsValid() {
            Assert.Operation.Message( $"AddressableHandle {this} must be valid" ).Valid( Handle.IsValid() );
        }
        protected internal void Assert_IsSucceeded() {
            Assert.Operation.Message( $"AddressableHandle {this} must be succeeded" ).Valid( Handle.IsSucceeded() );
        }
        protected internal void Assert_IsNotValid() {
            Assert.Operation.Message( $"AddressableHandle {this} is already valid" ).Valid( !Handle.IsValid() );
        }

    }
    // AddressableAssetHandle
    public abstract class AddressableAssetHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressableAssetHandle() {
        }

        // Release
        public void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
        public void ReleaseSafe() {
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    // AddressablePrefabHandle
    public abstract class AddressablePrefabHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressablePrefabHandle() {
        }

        // Release
        public void Release() {
            Assert_IsValid();
            Addressables.Release( Handle );
            Handle = default;
        }
        public void ReleaseSafe() {
            if (Handle.IsValid()) {
                Release();
            }
        }

    }
    // AddressableSceneHandle
    public abstract class AddressableSceneHandle : AddressableHandle<SceneInstance> {

        // Constructor
        public AddressableSceneHandle() {
        }

        // ActivateAsync
        public async Task<Scene> ActivateAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            var value = await Handle.GetResultAsync( cancellationToken );
            await value.ActivateAsync();
            cancellationToken.ThrowIfCancellationRequested();
            return value.Scene;
        }

        // UnloadAsync
        public async Task UnloadAsync(CancellationToken cancellationToken) {
            Assert_IsValid();
            await Addressables.UnloadSceneAsync( Handle ).Task.WaitAsync( cancellationToken );
            Handle = default;
        }
        public async Task UnloadSafeAsync(CancellationToken cancellationToken) {
            if (Handle.IsValid()) {
                await UnloadAsync( cancellationToken );
            }
        }

    }
    // AddressableInstanceHandle
    public class AddressableInstanceHandle<T> : AddressableHandle<T> where T : notnull {

        // Constructor
        public AddressableInstanceHandle() {
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

    }
}

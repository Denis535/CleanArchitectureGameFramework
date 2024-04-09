#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public abstract class AssetsHandleBase<T> where T : notnull, UnityEngine.Object {

        protected AsyncOperationHandle<IList<T>> AssetsHandle { get; private set; }
        public bool IsValid => AssetsHandle.IsValid();
        public bool IsSucceeded => AssetsHandle.IsValid() && AssetsHandle.IsSucceeded();
        public bool IsFailed => AssetsHandle.IsValid() && AssetsHandle.IsFailed();
        public IReadOnlyList<T> Assets {
            get {
                Assert.Operation.Message( $"AssetsHandle {this} must be valid" ).Valid( AssetsHandle.IsValid() );
                Assert.Operation.Message( $"AssetsHandle {this} must be succeeded" ).Valid( AssetsHandle.IsSucceeded() );
                return (IReadOnlyList<T>) AssetsHandle.Result;
            }
        }

        // Constructor
        public AssetsHandleBase() {
        }

        // LoadAssetsAsync
        protected Task<IReadOnlyList<T>> LoadAssetsAsync(AsyncOperationHandle<IList<T>> assetsHandle, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetsHandle {this} is already valid" ).Valid( !AssetsHandle.IsValid() );
            AssetsHandle = assetsHandle;
            return GetAssetsAsync( cancellationToken );
        }
        public async Task<IReadOnlyList<T>> GetAssetsAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetsHandle {this} must be valid" ).Valid( AssetsHandle.IsValid() );
            if (AssetsHandle.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await AssetsHandle.Task.WaitAsync( cancellationToken );
                if (AssetsHandle.Status is AsyncOperationStatus.Succeeded) {
                    return (IReadOnlyList<T>) result;
                }
            }
            throw AssetsHandle.OperationException;
        }

        // Release
        public void Release() {
            Assert.Operation.Message( $"AssetsHandle {this} must be valid" ).Valid( AssetsHandle.IsValid() );
            Addressables.Release( AssetsHandle );
            AssetsHandle = default;
        }
        public void ReleaseSafe() {
            if (AssetsHandle.IsValid()) {
                Release();
            }
        }

        // Utils
        public override string ToString() {
            return AssetsHandle.DebugName;
        }

    }
    // AssetsHandle
    public class AssetsHandle<T> : AssetsHandleBase<T> where T : notnull, UnityEngine.Object {

        public string[] Keys { get; }

        // Constructor
        public AssetsHandle(string[] keys) {
            Keys = keys;
        }

        // LoadAssetsAsync
        public Task<IReadOnlyList<T>> LoadAssetsAsync(CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetsHandle {this} is already valid" ).Valid( !AssetsHandle.IsValid() );
            return LoadAssetsAsync( Addressables.LoadAssetsAsync<T>( Keys.AsEnumerable(), null, Addressables.MergeMode.Union ), cancellationToken );
        }

    }
    // DynamicAssetsHandle
    public class DynamicAssetsHandle<T> : AssetsHandleBase<T> where T : notnull, UnityEngine.Object {

        private string[]? keys;

        [AllowNull]
        public string[] Keys {
            get {
                Assert.Operation.Message( $"AssetsHandle {this} must be valid" ).Valid( AssetsHandle.IsValid() );
                return keys!;
            }
            private set {
                keys = value;
            }
        }

        // Constructor
        public DynamicAssetsHandle() {
        }

        // LoadAssetsAsync
        public Task<IReadOnlyList<T>> LoadAssetsAsync(string[] keys, CancellationToken cancellationToken) {
            Assert.Operation.Message( $"AssetsHandle {this} is already valid" ).Valid( !AssetsHandle.IsValid() );
            return LoadAssetsAsync( Addressables.LoadAssetsAsync<T>( (Keys = keys).AsEnumerable(), null, Addressables.MergeMode.Union ), cancellationToken );
        }

    }
}

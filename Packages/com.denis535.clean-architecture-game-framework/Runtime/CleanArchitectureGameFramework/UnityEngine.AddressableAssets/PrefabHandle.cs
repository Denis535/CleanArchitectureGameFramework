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

    public class PrefabHandle {

        private AsyncOperationHandle<GameObject>? handle;

        public string Key { get; }
        public bool IsActive => handle != null;
        public bool IsValid => handle != null && handle.Value.IsValid();
        public bool IsSucceeded => handle != null && handle.Value.IsValid() && handle.Value.IsSucceeded();
        public bool IsFailed => handle != null && handle.Value.IsValid() && handle.Value.IsFailed();
        public GameObject Instance {
            get {
                Assert.Operation.Message( $"SceneHandle {this} must be active" ).Valid( handle != null );
                Assert.Operation.Message( $"SceneHandle {this} must be valid" ).Valid( handle.Value.IsValid() );
                Assert.Operation.Message( $"SceneHandle {this} must be succeeded" ).Valid( handle.Value.IsSucceeded() );
                return handle.Value.Result;
            }
        }

        // Constructor
        public PrefabHandle(string key) {
            Key = key;
        }

        // InstantiateAsync
        public async Task<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Transform? parent, CancellationToken cancellationToken) {
            if (handle == null) {
                handle = Addressables.InstantiateAsync( key, new InstantiationParameters( position, rotation, parent ) );
            }
            if (handle.Value.Status is AsyncOperationStatus.None or AsyncOperationStatus.Succeeded) {
                var result = await handle.Value.Task.WaitAsync( cancellationToken ).ConfigureAwait( false );
                if (handle.Value.Status is AsyncOperationStatus.Succeeded) {
                    return result;
                }
            }
            throw handle.Value.OperationException;
        }
        public void ReleaseInstance() {
            if (handle != null) {
                Addressables.ReleaseInstance( handle.Value );
                handle = null;
            }
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
}

//#nullable enable
//namespace UnityEngine.AddressableAssets {
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.Diagnostics.CodeAnalysis;
//    using System.Threading;
//    using System.Threading.Tasks;
//    using UnityEngine;
//    using UnityEngine.ResourceManagement.AsyncOperations;

//    public abstract class AddressableListHandle<T> {

//        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
//        public string[] Keys { get; }
//        public bool IsValid => Handle.IsValid();
//        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
//        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
//        public IReadOnlyList<T> Result {
//            get {
//                this.Assert_IsValid();
//                this.Assert_IsSucceeded();
//                return Handle.Result;
//            }
//        }

//        // Constructor
//        public AddressableListHandle(string[] keys) {
//            Keys = keys;
//        }

//        // LoadAsync
//        public abstract Task<IReadOnlyList<T>> LoadAsync(CancellationToken cancellationToken);

//        // GetResultAsync
//        public async Task<IReadOnlyList<T>> GetResultAsync(CancellationToken cancellationToken) {
//            this.Assert_IsValid();
//            return await Handle.GetResultAsync( cancellationToken );
//        }

//        // Release
//        public void Release() {
//            this.Assert_IsValid();
//            Addressables.Release( Handle );
//            Handle = default;
//        }
//        public void ReleaseSafe() {
//            if (Handle.IsValid()) {
//                Release();
//            }
//        }

//        // Utils
//        public override string ToString() {
//            return Handle.DebugName;
//        }

//    }
//    public abstract class DynamicAddressableListHandle<T> {

//        private string[]? keys;

//        protected AsyncOperationHandle<IReadOnlyList<T>> Handle { get; set; }
//        [AllowNull]
//        public string[] Keys {
//            get {
//                this.Assert_IsValid();
//                return keys!;
//            }
//            protected set {
//                keys = value;
//            }
//        }
//        public bool IsValid => Handle.IsValid();
//        public bool IsSucceeded => Handle.IsValid() && Handle.IsSucceeded();
//        public bool IsFailed => Handle.IsValid() && Handle.IsFailed();
//        public IReadOnlyList<T> Result {
//            get {
//                this.Assert_IsValid();
//                this.Assert_IsSucceeded();
//                return Handle.Result;
//            }
//        }

//        // Constructor
//        public DynamicAddressableListHandle() {
//        }

//        // LoadAsync
//        public abstract Task<IReadOnlyList<T>> LoadAsync(string[] keys, CancellationToken cancellationToken);

//        // GetResultAsync
//        public async Task<IReadOnlyList<T>> GetResultAsync(CancellationToken cancellationToken) {
//            this.Assert_IsValid();
//            return await Handle.GetResultAsync( cancellationToken );
//        }

//        // Release
//        public void Release() {
//            this.Assert_IsValid();
//            Addressables.Release( Handle );
//            Handle = default;
//        }
//        public void ReleaseSafe() {
//            if (Handle.IsValid()) {
//                Release();
//            }
//        }

//        // Utils
//        public override string ToString() {
//            return Handle.DebugName;
//        }

//    }
//}

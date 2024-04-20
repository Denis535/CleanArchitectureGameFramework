#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    public abstract class AddressableHandleBase2 : AddressableHandleBase {

        // Key
        public string Key { get; }

        // Constructor
        public AddressableHandleBase2(string key) {
            Key = key;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public abstract class AddressableListHandleBase2 : AddressableHandleBase {

        // Keys
        public string[] Keys { get; }

        // Constructor
        public AddressableListHandleBase2(string[] keys) {
            Keys = keys;
        }

        // Utils
        public override string ToString() {
            return string.Join( ", ", Keys );
        }

    }
    public abstract class DynamicAddressableHandleBase2 : AddressableHandleBase {

        private string? key;

        // Key
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

        // Constructor
        public DynamicAddressableHandleBase2() {
        }
        public DynamicAddressableHandleBase2(string key) {
            Key = key;
        }

        // Utils
        public override string ToString() {
            return Key;
        }

    }
    public abstract class DynamicAddressableListHandleBase2 : AddressableHandleBase {

        private string[]? keys;

        // Keys
        [AllowNull]
        public string[] Keys {
            get {
                Assert_IsValid();
                return keys!;
            }
            protected set {
                keys = value;
            }
        }

        // Constructor
        public DynamicAddressableListHandleBase2() {
        }
        public DynamicAddressableListHandleBase2(string[] keys) {
            Keys = keys;
        }

        // Utils
        public override string ToString() {
            return string.Join( ", ", Keys );
        }

    }
}

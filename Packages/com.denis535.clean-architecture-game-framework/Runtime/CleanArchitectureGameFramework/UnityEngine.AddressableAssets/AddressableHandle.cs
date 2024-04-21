#nullable enable
namespace UnityEngine.AddressableAssets {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class AddressableHandle : AddressableHandleBase {

        // Key
        public string Key { get; }

        // Constructor
        public AddressableHandle(string key) {
            Key = key;
        }

        // Utils
        public override string ToString() {
            return "AddressableHandle: " + Key;
        }

    }
    public abstract class DynamicAddressableHandle : AddressableHandleBase {

        // Key
        public string? Key { get; protected set; }

        // Constructor
        public DynamicAddressableHandle(string? key) {
            Key = key;
        }

        // Utils
        public override string ToString() {
            if (Key != null) {
                return "DynamicAddressableHandle: " + Key;
            } else {
                return "DynamicAddressableHandle";
            }
        }

    }
    public abstract class AddressableListHandle : AddressableListHandleBase {

        // Keys
        public string[] Keys { get; }

        // Constructor
        public AddressableListHandle(string[] keys) {
            Keys = keys;
        }

        // Utils
        public override string ToString() {
            return "AddressableListHandle: " + string.Join( ", ", Keys );
        }

    }
    public abstract class DynamicAddressableListHandle : AddressableListHandleBase {

        // Keys
        public string[]? Keys { get; protected set; }

        // Constructor
        public DynamicAddressableListHandle(string[]? keys) {
            Keys = keys;
        }

        // Utils
        public override string ToString() {
            if (Keys != null) {
                return "DynamicAddressableListHandle: " + string.Join( ", ", Keys );
            } else {
                return "DynamicAddressableListHandle";
            }
        }

    }
}

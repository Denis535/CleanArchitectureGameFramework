#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ValueTracker<T> {

        private Option<T> Value { get; set; }

        // Constructor
        public ValueTracker() {
            Value = default;
        }

        // IsChanged
        public bool IsChanged(T value) {
            if (!Value.Equals( value )) {
                Value = new Option<T>( value );
                return true;
            } else {
                return false;
            }
        }
        public bool IsChanged(T value, out T newValue) {
            if (!Value.Equals( value )) {
                Value = new Option<T>( value );
                newValue = Value.Value;
                return true;
            } else {
                newValue = Value.Value;
                return false;
            }
        }
        public bool IsChanged(T value, out T newValue, out Option<T> prevValue) {
            if (!Value.Equals( value )) {
                prevValue = Value;
                Value = new Option<T>( value );
                newValue = Value.Value;
                return true;
            } else {
                prevValue = Value;
                newValue = Value.Value;
                return false;
            }
        }

        // Utils
        public override string ToString() {
            return "ValueTracker ({0})".Format( Value );
        }

    }
}

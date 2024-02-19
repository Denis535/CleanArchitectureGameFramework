#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ValueTracker<T> {

        private readonly Func<T> getter;
        private Option<T> Value { get; set; }

        // Constructor
        public ValueTracker(Func<T> getter) {
            this.getter = getter;
            this.Value = default;
        }

        // IsChanged
        public bool IsChanged() {
            var value = getter();
            if (!Value.Equals( value )) {
                Value = new Option<T>( value );
                return true;
            } else {
                return false;
            }
        }
        public bool IsChanged(out T newValue) {
            var value = getter();
            if (!Value.Equals( value )) {
                Value = new Option<T>( value );
                newValue = Value.Value;
                return true;
            } else {
                newValue = Value.Value;
                return false;
            }
        }
        public bool IsChanged(out T newValue, out Option<T> prevValue) {
            var value = getter();
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

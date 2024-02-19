#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ValueTracker2<T, TObj> {

        private readonly Func<TObj, T> getter;
        public Option<T> Value { get; set; }

        // Constructor
        public ValueTracker2(Func<TObj, T> getter) {
            this.getter = getter;
            this.Value = default;
        }

        // IsChanged
        public bool IsChanged(TObj obj) {
            var value = getter( obj );
            if (!Value.Equals( value )) {
                Value = new Option<T>( value );
                return true;
            } else {
                return false;
            }
        }
        public bool IsChanged(TObj obj, out T newValue) {
            var value = getter( obj );
            if (!Value.Equals( value )) {
                Value = new Option<T>( value );
                newValue = Value.Value;
                return true;
            } else {
                newValue = Value.Value;
                return false;
            }
        }
        public bool IsChanged(TObj obj, out T newValue, out Option<T> prevValue) {
            var value = getter( obj );
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

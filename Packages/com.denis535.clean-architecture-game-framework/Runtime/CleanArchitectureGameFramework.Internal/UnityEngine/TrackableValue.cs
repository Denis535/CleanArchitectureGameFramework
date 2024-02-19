#nullable enable
namespace UnityEngine {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public readonly struct TrackableValue<T> {

        private readonly TrackableValueSource<T> source;

        public T Value => source.Value;
        public bool IsChanged => source.IsChanged;

        // Constructor
        internal TrackableValue(TrackableValueSource<T> source) {
            this.source = source;
        }

        // Peek
        public readonly bool Peek(out T value) {
            return source.Peek( out value );
        }

        // Consume
        public readonly bool Consume(out T value) {
            return source.Consume( out value );
        }

        // Utils
        public readonly override string ToString() {
            return "TrackableValue ({0}, {1})".Format( source.Value, source.IsChanged );
        }

    }
    public class TrackableValueSource<T> {

        internal T Value { get; private set; }
        internal bool IsChanged { get; private set; }
        public TrackableValue<T> Trackable => new TrackableValue<T>( this );

        // Constructor
        public TrackableValueSource(T value) {
            Value = value;
            IsChanged = true;
        }

        // SetValue
        public void SetValue(T value) {
            Assert.Operation.Message( $"You are trying to set value but it's already changed" ).Valid( !IsChanged );
            if (!EqualityComparer<T>.Default.Equals( Value, value )) {
                (Value, IsChanged) = (value, true);
            }
        }

        // Peek
        internal bool Peek(out T value) {
            value = Value;
            return IsChanged;
        }

        // Consume
        internal bool Consume(out T value) {
            value = Value;
            var isChanged = IsChanged;
            IsChanged = false;
            return isChanged;
        }

        // Utils
        public override string ToString() {
            return "TrackableValueSource ({0}, {1})".Format( Value, IsChanged );
        }

    }
}

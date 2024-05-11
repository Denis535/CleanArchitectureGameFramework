#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class SlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull {

        public abstract T? Child { get; }

        public SlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public abstract void Set(T child);
        public abstract void Clear();

    }
    public abstract class ListSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull {

        public abstract IReadOnlyList<T> Children { get; }

        public ListSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public abstract void Add(T child);
        public abstract void Remove(T child);
        public abstract void Clear();

    }
    public abstract class StackSlotWrapper<T> : VisualElementWrapper<VisualElement> where T : notnull {

        public abstract IReadOnlyCollection<T> Children { get; }

        public StackSlotWrapper(VisualElement visualElement) : base( visualElement ) {
        }

        public abstract void Push(T child);
        public abstract T Peek();
        public abstract T Pop();

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public interface IVisualElementWrapper {
        internal VisualElement VisualElement { get; }
    }
    public interface IVisualElementWrapper<out T> : IVisualElementWrapper where T : VisualElement {
        internal new T VisualElement { get; }
    }
}

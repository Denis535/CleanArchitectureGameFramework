#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public interface IVisualElementWrapper<out T> where T : VisualElement {
        internal T VisualElement { get; }
    }
}

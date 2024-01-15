#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal interface IUIViewable {
        UIViewBase View { get; }
    }
}

#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal interface IUIViewableWidget {
        UIViewBase View { get; }
    }
}

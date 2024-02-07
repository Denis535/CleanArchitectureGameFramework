#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum UIWidgetState {
        // Inactive
        Unattached,
        // Active
        Attaching,
        Attached,
        Detaching,
        // Inactive
        Detached
    }
}

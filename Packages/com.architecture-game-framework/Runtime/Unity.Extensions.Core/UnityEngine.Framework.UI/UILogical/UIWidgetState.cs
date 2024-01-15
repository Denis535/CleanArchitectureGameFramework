#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public enum UIWidgetState {
        // Inactive
        Unattached,
        // Active
        Attaching,
        Attached,
        Detaching,
        // Inactive
        Detached,
    }
}

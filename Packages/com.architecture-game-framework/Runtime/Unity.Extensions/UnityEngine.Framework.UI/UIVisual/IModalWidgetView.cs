#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IModalWidgetView {
    }
    public static class IModalWidgetViewExtensions {

        public static bool IsModal(this UIViewBase view) {
            return view is IModalWidgetView;
        }

    }
}

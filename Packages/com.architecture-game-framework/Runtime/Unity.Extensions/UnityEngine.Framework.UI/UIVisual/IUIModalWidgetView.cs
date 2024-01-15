#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IUIModalWidgetView {
    }
    public static class IUIModalWidgetViewExtensions {

        public static bool IsModal(this UIWidgetViewBase view) {
            return view is IUIModalWidgetView;
        }

    }
}

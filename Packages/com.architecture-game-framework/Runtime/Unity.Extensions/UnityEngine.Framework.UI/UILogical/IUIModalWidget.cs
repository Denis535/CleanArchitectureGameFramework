#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IUIModalWidget {
    }
    public static class IUIModalWidgetExtensions {

        public static bool IsModal(this UIWidgetBase widget) {
            return widget is IUIModalWidget;
        }

    }
}

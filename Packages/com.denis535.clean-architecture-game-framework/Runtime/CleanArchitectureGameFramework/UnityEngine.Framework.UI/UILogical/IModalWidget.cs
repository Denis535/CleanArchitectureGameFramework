#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IModalWidget {
    }
    public static class IModalWidgetExtensions {

        public static bool IsModal(this UIWidgetBase widget) {
            return widget is IModalWidget;
        }

    }
}

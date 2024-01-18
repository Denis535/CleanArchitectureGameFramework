#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IUIModalView {
    }
    public static class IUIModalViewExtensions {

        public static bool IsModal(this UIViewBase view) {
            return view is IUIModalView;
        }

    }
}

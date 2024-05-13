#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class UIViewExtensions {

        // GetParent
        public static UIViewBase? GetParent(this UIViewBase view) {
            return view.VisualElement.GetAncestors().Select( i => i.userData ).OfType<UIViewBase>().FirstOrDefault();
        }
        public static IEnumerable<UIViewBase> GetAncestors(this UIViewBase view) {
            return view.VisualElement.GetAncestors().Select( i => i.userData ).OfType<UIViewBase>();
        }
        public static IEnumerable<UIViewBase> GetAncestorsAndSelf(this UIViewBase view) {
            return view.VisualElement.GetAncestorsAndSelf().Select( i => i.userData ).OfType<UIViewBase>();
        }

        // GetChildren
        public static IEnumerable<UIViewBase> GetChildren(this UIViewBase view) {
            return view.VisualElement.GetDescendants( i => i.userData == view || i.userData is not UIViewBase ).Select( i => i.userData ).OfType<UIViewBase>();
        }
        public static IEnumerable<UIViewBase> GetDescendants(this UIViewBase view) {
            return view.VisualElement.GetDescendants().Select( i => i.userData ).OfType<UIViewBase>();
        }
        public static IEnumerable<UIViewBase> GetDescendantsAndSelf(this UIViewBase view) {
            return view.VisualElement.GetDescendantsAndSelf().Select( i => i.userData ).OfType<UIViewBase>();
        }

        // GetVisualElement
        public static VisualElement __GetVisualElement__(this UIViewBase view) {
            // try not to use this method
            return view.VisualElement;
        }

        // Add
        public static void Add(this VisualElement element, UIViewBase view) {
            element.Add( view.VisualElement );
        }
        public static void Remove(this VisualElement element, UIViewBase view) {
            element.Remove( view.VisualElement );
        }
        public static bool Contains(this VisualElement element, UIViewBase view) {
            return element.Contains( view.VisualElement );
        }

    }
}

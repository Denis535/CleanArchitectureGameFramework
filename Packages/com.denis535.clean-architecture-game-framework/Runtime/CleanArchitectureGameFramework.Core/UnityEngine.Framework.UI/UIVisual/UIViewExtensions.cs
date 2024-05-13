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
            return view.__GetVisualElement__().GetAncestors().Select( i => i.userData ).OfType<UIViewBase>().FirstOrDefault();
        }
        public static IEnumerable<UIViewBase> GetAncestors(this UIViewBase view) {
            return view.__GetVisualElement__().GetAncestors().Select( i => i.userData ).OfType<UIViewBase>();
        }
        public static IEnumerable<UIViewBase> GetAncestorsAndSelf(this UIViewBase view) {
            return view.__GetVisualElement__().GetAncestorsAndSelf().Select( i => i.userData ).OfType<UIViewBase>();
        }

        // GetChildren
        public static IEnumerable<UIViewBase> GetChildren(this UIViewBase view) {
            return view.__GetVisualElement__().GetDescendants( i => i.userData == view || i.userData is not UIViewBase ).Select( i => i.userData ).OfType<UIViewBase>();
        }
        public static IEnumerable<UIViewBase> GetDescendants(this UIViewBase view) {
            return view.__GetVisualElement__().GetDescendants().Select( i => i.userData ).OfType<UIViewBase>();
        }
        public static IEnumerable<UIViewBase> GetDescendantsAndSelf(this UIViewBase view) {
            return view.__GetVisualElement__().GetDescendantsAndSelf().Select( i => i.userData ).OfType<UIViewBase>();
        }

        // Add
        public static void Add(this VisualElement element, UIViewBase view) {
            element.Add( view.__GetVisualElement__() );
        }
        public static void Remove(this VisualElement element, UIViewBase view) {
            element.Remove( view.__GetVisualElement__() );
        }
        public static bool Contains(this VisualElement element, UIViewBase view) {
            return element.Contains( view.__GetVisualElement__() );
        }

    }
}

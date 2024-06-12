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
            return GetChildren( view.VisualElement );
            static IEnumerable<UIViewBase> GetChildren(VisualElement element) {
                foreach (var child in element.Children()) {
                    if (child.userData is UIViewBase) {
                        yield return (UIViewBase) child.userData;
                    } else {
                        foreach (var i in GetChildren( child )) yield return i;
                    }
                }
            }
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
        public static T __GetVisualElement__<T>(this UIViewBase view) where T : VisualElement {
            // try not to use this method
            return (T) view.VisualElement;
        }

        // Focus
        public static void Focus(this UIViewBase view) {
            try {
                if (view.VisualElement.focusable) {
                    view.VisualElement.Focus();
                } else {
                    view.VisualElement.focusable = true;
                    view.VisualElement.delegatesFocus = true;
                    view.VisualElement.Focus(); // sometimes it throws an error
                    view.VisualElement.delegatesFocus = false;
                    view.VisualElement.focusable = false;
                }
            } catch {
            }
        }
        public static bool LoadFocus(this UIViewBase view) {
            var focusedElement = view.LoadFocusedElement();
            if (focusedElement != null) {
                focusedElement.Focus();
                return true;
            }
            return false;
        }
        public static void SaveFocus(this UIViewBase view) {
            var focusedElement = GetFocusedElement( view );
            view.SaveFocusedElement( focusedElement );
        }

        // GetFocusedElement
        public static VisualElement? GetFocusedElement(this UIViewBase view) {
            var focusedElement = (VisualElement?) view.VisualElement.focusController?.focusedElement;
            if (focusedElement != null && (view.VisualElement == focusedElement || view.VisualElement.Contains( focusedElement ))) return focusedElement;
            return null;
        }
        public static bool HasFocusedElement(this UIViewBase view) {
            var focusedElement = (VisualElement?) view.VisualElement.focusController?.focusedElement;
            if (focusedElement != null && (view.VisualElement == focusedElement || view.VisualElement.Contains( focusedElement ))) return true;
            return false;
        }

    }
    public static class VisualElementExtensions {

        // GetView
        public static UIViewBase GetView(this VisualElement element) {
            return (UIViewBase) element.userData ?? throw Exceptions.Internal.NullReference( $"View is null" );
        }
        public static T GetView<T>(this VisualElement element) where T : UIViewBase {
            return (T) element.userData ?? throw Exceptions.Internal.NullReference( $"View is null" );
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

        // AsObservable
        public static UIObservable<T> AsObservable<T>(this VisualElement element) where T : notnull, EventBase<T>, new() {
            return new UIObservable<T>( element );
        }

    }
    public struct UIObservable<T> where T : notnull, EventBase<T>, new() {

        private readonly VisualElement visualElement;

        public UIObservable(VisualElement visualElement) {
            this.visualElement = visualElement;
        }

        public void Register(EventCallback<T> callback) {
            visualElement.RegisterCallback( callback );
        }
        public void RegisterOnce(EventCallback<T> callback) {
            visualElement.RegisterCallbackOnce( callback );
        }
        public void Unregister(EventCallback<T> callback) {
            visualElement.UnregisterCallback( callback );
        }

    }
}

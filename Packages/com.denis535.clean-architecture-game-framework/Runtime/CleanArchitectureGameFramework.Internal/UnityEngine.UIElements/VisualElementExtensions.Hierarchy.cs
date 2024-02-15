#nullable enable
namespace UnityEngine.UIElements {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static partial class VisualElementExtensions {

        // IsAncestorOf
        public static bool IsAncestorOf(this VisualElement element, VisualElement descendant) {
            return element.Contains( descendant );
        }

        // IsDescendantOf
        public static bool IsDescendantOf(this VisualElement element, VisualElement ancestor) {
            return ancestor.Contains( element );
        }

        // GetAncestors
        public static IEnumerable<VisualElement> GetAncestors(this VisualElement element) {
            while (element.parent != null) {
                yield return element.parent;
                element = element.parent;
            }
        }
        public static IEnumerable<VisualElement> GetAncestorsAndSelf(this VisualElement element) {
            yield return element;
            foreach (var ancestor in element.GetAncestors()) {
                yield return ancestor;
            }
        }

        // GetDescendants
        public static IEnumerable<VisualElement> GetDescendants(this VisualElement element) {
            foreach (var child in element.Children()) {
                yield return child;
                foreach (var descendant in child.GetDescendants()) {
                    yield return descendant;
                }
            }
        }
        public static IEnumerable<VisualElement> GetDescendantsAndSelf(this VisualElement element) {
            yield return element;
            foreach (var descendant in element.GetDescendants()) {
                yield return descendant;
            }
        }

        // GetDescendants
        public static IEnumerable<VisualElement> GetDescendants(this VisualElement element, Func<VisualElement, bool> descentIntoElement) {
            foreach (var child in element.Children()) {
                yield return child;
                if (descentIntoElement( child )) {
                    foreach (var descendant in child.GetDescendants( descentIntoElement )) {
                        yield return descendant;
                    }
                }
            }
        }
        public static IEnumerable<VisualElement> GetDescendantsAndSelf(this VisualElement element, Func<VisualElement, bool> descentIntoElement) {
            yield return element;
            if (descentIntoElement( element )) {
                foreach (var descendant in element.GetDescendants( descentIntoElement )) {
                    yield return descendant;
                }
            }
        }

        // Add
        //public static void Add(this VisualElement element, VisualElement child) {
        //    element.Add( child );
        //}
        //public static void Insert(this VisualElement element, int index, VisualElement child) {
        //    element.Insert( index, child );
        //}
        public static void InsertBefore(this VisualElement element, VisualElement index, VisualElement child) {
            var index_ = element.IndexOf( index );
            element.Insert( index_, child );
        }
        public static void InsertAfter(this VisualElement element, VisualElement index, VisualElement child) {
            var index_ = element.IndexOf( index ) + 1;
            element.Insert( index_, child );
        }
        //public static void Remove(this VisualElement element, VisualElement child) {
        //    element.Remove( child );
        //}
        //public static void RemoveAt(this VisualElement element, int index) {
        //    element.RemoveAt( index );
        //}
        public static void Replace(this VisualElement element, VisualElement oldChild, VisualElement newChild) {
            var index = element.IndexOf( oldChild );
            element.RemoveAt( index );
            element.Insert( index, newChild );
        }

        // Add
        public static void Add(this VisualElement element, params VisualElement[] children) {
            foreach (var child in children) {
                element.Add( child );
            }
        }
        public static void Insert(this VisualElement element, int index, params VisualElement[] children) {
            foreach (var child in children) {
                element.Insert( index++, child );
            }
        }
        public static void InsertBefore(this VisualElement element, VisualElement index, params VisualElement[] children) {
            var index_ = element.IndexOf( index );
            foreach (var child in children) {
                element.Insert( index_++, child );
            }
        }
        public static void InsertAfter(this VisualElement element, VisualElement index, params VisualElement[] children) {
            var index_ = element.IndexOf( index ) + 1;
            foreach (var child in children) {
                element.Insert( index_++, child );
            }
        }
        public static void Remove(this VisualElement element, params VisualElement[] children) {
            foreach (var child in children) {
                element.Remove( child );
            }
        }
        public static void RemoveAt(this VisualElement element, int index, int count) {
            for (var i = 0; i < count; i++) {
                element.RemoveAt( index );
            }
        }
        public static void Replace(this VisualElement element, VisualElement oldChild, params VisualElement[] newChildren) {
            var index = element.IndexOf( oldChild );
            element.RemoveAt( index );
            foreach (var child in newChildren) {
                element.Insert( index++, child );
            }
        }

    }
}

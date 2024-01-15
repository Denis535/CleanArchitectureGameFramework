#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : IDisposable {

        // System
        public bool IsDisposed { get; private set; }
        // VisualElement
        public abstract VisualElement VisualElement { get; }

        // Constructor
        public UIViewBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"View {this} must be alive" ).Alive( !IsDisposed );
            Assert.Object.Message( $"View {this} must be non-attached" ).Valid( VisualElement.panel == null );
            IsDisposed = true;
            if (VisualElement.visualTreeAssetSource != null) Addressables2.Release( VisualElement.visualTreeAssetSource );
        }

    }
}

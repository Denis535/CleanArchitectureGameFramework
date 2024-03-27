#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.UIElements;

    public abstract class UIViewBase : IDisposable {

        private VisualElement visualElement = default!;

        // System
        public bool IsDisposed { get; private set; }
        // View
        protected internal VisualElement VisualElement {
            get {
                return visualElement;
            }
            protected init {
                visualElement = value;
                visualElement.OnAttachToPanel( evt => {
                    ViewAttachEvent.Dispatch( visualElement );
                } );
                visualElement.OnDetachFromPanel( evt => {
                    ViewDetachEvent.Dispatch( visualElement );
                } );
            }
        }

        // Constructor
        public UIViewBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"View {this} must be alive" ).Alive( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( VisualElement.panel == null );
            IsDisposed = true;
            if (VisualElement.visualTreeAssetSource != null) Addressables2.Release( VisualElement.visualTreeAssetSource );
        }

    }
}

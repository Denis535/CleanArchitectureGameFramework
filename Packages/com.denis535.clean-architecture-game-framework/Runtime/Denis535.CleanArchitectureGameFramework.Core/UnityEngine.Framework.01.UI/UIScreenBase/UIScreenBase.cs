#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase : DisposableBase {

        // Document
        protected UIDocument Document { get; }
        // AudioSource
        protected AudioSource AudioSource { get; }
        // Tree
        private Tree<UIWidgetBase> Tree { get; } = new Tree<UIWidgetBase>();
        // Widget
        protected internal UIWidgetBase? Widget {
            get {
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return Tree.Root;
            }
        }

        // Constructor
        public UIScreenBase(UIDocument document, AudioSource audioSource) {
            Document = document;
            AudioSource = audioSource;
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            base.Dispose();
        }

        // SetWidget
        protected internal virtual void SetWidget(UIWidgetBase? widget, object? argument = null) {
            if (widget != null) {
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be non-disposed" ).Valid( !widget.IsDisposed );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be inactive" ).Valid( widget.State is NodeBase.State_.Inactive );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.IsViewable );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.View != null );
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
                Tree.SetRoot( widget );
                Document.rootVisualElement.Add( Widget!.View );
            } else {
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
                if (Document && Document.rootVisualElement != null) Document.rootVisualElement.Remove( Widget.View );
                Tree.SetRoot( null );
            }
        }

    }
}

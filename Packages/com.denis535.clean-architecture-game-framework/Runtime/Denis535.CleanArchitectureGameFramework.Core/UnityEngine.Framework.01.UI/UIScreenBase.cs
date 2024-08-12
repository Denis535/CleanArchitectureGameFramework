#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase : Disposable {

        // Document
        protected UIDocument Document { get; }
        // AudioSource
        protected AudioSource AudioSource { get; }
        // Widget
        protected internal UIWidgetBase? Widget { get; private set; }

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

        // AddWidget
        protected internal virtual void AddWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be non-disposed" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be inactive" ).Valid( widget.State is UIWidgetState.Inactive );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.IsViewable );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.View != null );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            Widget = widget;
            widget.Activate( this, argument );
            Document.rootVisualElement.Add( widget.View );
        }
        protected internal virtual void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be non-disposed" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be active" ).Valid( widget.State is UIWidgetState.Active );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.IsViewable );
            Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.View != null );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have {widget} widget" ).Valid( Widget == widget );
            if (Document && Document.rootVisualElement != null) Document.rootVisualElement.Remove( widget.View );
            widget.Deactivate( this, argument );
            Widget = null;
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIScreenBase : Disposable {

        // Widget
        protected internal UIWidgetBase Widget { get; private set; } = default!;

        // Constructor
        public UIScreenBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            base.Dispose();
        }

        // AddWidget
        protected internal virtual void AddWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( widget.State is UIWidgetState.Inactive );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            Widget = widget;
            widget.Activate( this, argument );
        }
        protected internal virtual void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( widget.State is UIWidgetState.Active );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have {widget} widget" ).Valid( Widget == widget );
            widget.Deactivate( this, argument );
            Widget = default!;
        }

    }
}

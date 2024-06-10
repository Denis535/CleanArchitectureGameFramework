#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIScreenBase : Disposable, IUILogicalElement {

        private readonly Lock @lock = new Lock();

        // Widget
        public UIWidgetBase Widget { get; private set; } = default!;

        // Constructor
        public UIScreenBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            base.Dispose();
        }

        // AddWidget
        public virtual void AddWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( widget.State is UIWidgetState.Inactive );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            using (@lock.Enter()) {
                Widget = widget;
                widget.Parent = null;
                widget.Activate( this, argument );
            }
        }
        public virtual void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( widget.State is UIWidgetState.Active );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have {widget} widget" ).Valid( Widget == widget );
            using (@lock.Enter()) {
                widget.Deactivate( this, argument );
                widget.Parent = null;
                Widget = null!;
            }
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIScreenBase : Disposable, IUILogicalElement {

        internal readonly Lock @lock = new Lock();

        // Widget
        public UIWidgetBase Widget { get; internal set; } = default!;

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
            this.AddWidgetInternal( widget, argument );
        }
        public virtual void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( !widget.IsDisposed );
            Assert.Argument.Message( $"Argument 'widget' must be valid" ).Valid( widget.State is UIWidgetState.Active );
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have {widget} widget" ).Valid( Widget == widget );
            this.RemoveWidgetInternal( widget, argument );
        }

    }
}

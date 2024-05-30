#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIScreenBase : Disposable, IUILogicalElement {

        private readonly Lock @lock = new Lock();

        // Widget
        public UIWidgetBase? Widget { get; private set; }

        // Constructor
        public UIScreenBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AttachWidget
        public virtual void AttachWidget(UIWidgetBase widget, object? argument = null) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            using (@lock.Enter()) {
                Widget = widget;
                widget.Parent = null;
                if (true) {
                    Widget.AttachToScreen( this, argument );
                }
            }
        }
        public virtual void DetachWidget(UIWidgetBase widget, object? argument = null) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Operation.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
            Assert.Operation.Message( $"Screen {this} must have widget {widget} widget" ).Valid( Widget == widget );
            using (@lock.Enter()) {
                if (true) {
                    Widget.DetachFromScreen( this, argument );
                }
                widget.Parent = null;
                Widget = null;
            }
            if (widget.DisposeWhenDetach) {
                widget.Dispose();
            }
        }

    }
}

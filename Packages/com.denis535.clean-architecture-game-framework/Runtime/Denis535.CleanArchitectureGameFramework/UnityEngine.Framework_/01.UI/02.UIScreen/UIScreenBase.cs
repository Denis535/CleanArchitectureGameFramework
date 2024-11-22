#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.TreeMachine;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase : DisposableBase, ITree<UIWidgetBase> {

        // Document
        protected internal UIDocument Document { get; }
        // AudioSource
        protected internal AudioSource AudioSource { get; }
        // Widget
        UIWidgetBase? ITree<UIWidgetBase>.Root { get => Widget; set => Widget = value; }
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

        // SetWidget
        void ITree<UIWidgetBase>.SetRoot(UIWidgetBase? root, object? argument) {
            SetWidget( root, argument );
        }
        protected internal virtual void SetWidget(UIWidgetBase? widget, object? argument = null) {
            if (widget != null) {
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be non-disposed" ).Valid( !widget.IsDisposed );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be inactive" ).Valid( widget.Activity is UIWidgetBase.Activity_.Inactive );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.IsViewable );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.View != null );
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
                ITree<UIWidgetBase>.SetRoot( this, widget, argument );
            } else {
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Screen {this} must have widget" ).Valid( Widget != null );
                ITree<UIWidgetBase>.SetRoot( this, null, argument );
            }
        }

    }
}

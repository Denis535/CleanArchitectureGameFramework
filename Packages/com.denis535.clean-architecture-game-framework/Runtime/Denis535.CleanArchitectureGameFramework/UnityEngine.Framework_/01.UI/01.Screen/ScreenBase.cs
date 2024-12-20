#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.TreeMachine;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class ScreenBase : DisposableBase, ITree<WidgetBase> {

        private WidgetBase? widget;

        // Document
        protected internal UIDocument Document { get; }
        // AudioSource
        protected internal AudioSource AudioSource { get; }
        // Widget
        WidgetBase? ITree<WidgetBase>.Root { get => Widget; set => Widget = value; }
        protected internal WidgetBase? Widget {
            get {
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return widget;
            }
            private set {
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                widget = value;
            }
        }

        // Constructor
        public ScreenBase(UIDocument document, AudioSource audioSource) {
            Document = document;
            AudioSource = audioSource;
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Screen {this} must have no widget" ).Valid( Widget == null );
            Assert.Operation.Message( $"Screen {this} must be released" ).Valid( !Document || Document.rootVisualElement == null || Document.rootVisualElement.childCount == 0 );
            Assert.Operation.Message( $"Screen {this} must be released" ).Valid( !AudioSource || AudioSource.clip == null );
            base.Dispose();
        }

        // SetWidget
        void ITree<WidgetBase>.SetRoot(WidgetBase? root, object? argument) {
            SetWidget( root, argument );
        }
        protected internal virtual void SetWidget(WidgetBase? widget, object? argument = null) {
            if (widget != null) {
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be non-disposed" ).Valid( !widget.IsDisposed );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be inactive" ).Valid( widget.Activity is WidgetBase.Activity_.Inactive );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.IsViewable );
                Assert.Argument.Message( $"Argument 'widget' ({widget}) must be viewable" ).Valid( widget.View != null );
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                ITree<WidgetBase>.SetRoot( this, widget, argument );
            } else {
                Assert.Operation.Message( $"Screen {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                ITree<WidgetBase>.SetRoot( this, widget, argument );
            }
        }
        void ITree<WidgetBase>.AddRoot(WidgetBase widget, object? argument) {
            ITree<WidgetBase>.AddRoot( this, widget, argument );
        }
        void ITree<WidgetBase>.RemoveRoot(WidgetBase widget, object? argument) {
            ITree<WidgetBase>.RemoveRoot( this, widget, argument );
            widget.Dispose();
        }

    }
}

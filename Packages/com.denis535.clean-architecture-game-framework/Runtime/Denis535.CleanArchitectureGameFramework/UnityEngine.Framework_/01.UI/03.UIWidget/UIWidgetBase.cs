#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.TreeMachine;
    using UnityEngine;

    public abstract class UIWidgetBase : NodeBase2<UIWidgetBase>, IDisposable {

        private CancellationTokenSource? disposeCancellationTokenSource;

        // System
        public bool IsDisposed { get; private set; }
        public CancellationToken DisposeCancellationToken {
            get {
                if (disposeCancellationTokenSource == null) {
                    disposeCancellationTokenSource = new CancellationTokenSource();
                    if (IsDisposed) disposeCancellationTokenSource.Cancel();
                }
                return disposeCancellationTokenSource.Token;
            }
        }
        // Screen
        protected UIScreenBase? Screen {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Widget {this} must be active or activating or deactivating" ).Valid( Activity is Activity_.Active or Activity_.Activating or Activity_.Deactivating );
                return (UIScreenBase?) Tree;
            }
        }
        // View
        [MemberNotNullWhen( true, "View" )]
        public virtual bool IsViewable {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return false;
            }
        }
        protected internal virtual UIViewBase? View {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return null;
            }
        }

        // Constructor
        public UIWidgetBase() {
        }
        public virtual void Dispose() {
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

        // OnAttach
        protected override void OnAttach(object? argument) {
        }
        protected override void OnDetach(object? argument) {
        }

        // AddChild
        protected override void AddChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-disposed" ).Valid( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.AddChild( child, argument );
        }
        protected override void RemoveChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-disposed" ).Valid( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.RemoveChild( child, argument );
            child.Dispose();
        }

    }
    public abstract class UIWidgetBase<TView> : UIWidgetBase where TView : notnull, UIViewBase {

        private TView view = default!;

        // View
        public sealed override bool IsViewable {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return true;
            }
        }
        protected internal sealed override UIViewBase? View {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return View2;
            }
        }
        protected internal TView View2 {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return view;
            }
            init {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                view = value;
            }
        }

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            Assert.Operation.Message( $"Widget {this} must be released" ).Valid( View2.IsDisposed );
            base.Dispose();
        }

        // ShowSelf
        protected virtual void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-root" ).NotDisposed( !IsRoot );
            Assert.Operation.Message( $"Parent {Parent} must be viewable" ).NotDisposed( Parent.IsViewable );
            Parent.View.AddViewRecursive( View2 );
        }
        protected virtual void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-root" ).NotDisposed( !IsRoot );
            Assert.Operation.Message( $"Parent {Parent} must be viewable" ).NotDisposed( Parent.IsViewable );
            Parent.View.RemoveViewRecursive( View2 );
        }

    }
}

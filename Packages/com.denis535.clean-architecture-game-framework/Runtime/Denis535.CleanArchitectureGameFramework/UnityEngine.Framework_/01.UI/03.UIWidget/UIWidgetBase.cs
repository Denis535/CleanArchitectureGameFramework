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
        protected UIScreenBase? Screen => (UIScreenBase?) Tree;
        // View
        [MemberNotNullWhen( true, "View" )] public virtual bool IsViewable => false;
        protected internal virtual UIViewBase? View => null;

        // Constructor
        public UIWidgetBase() {
        }
        public virtual void Dispose() {
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            if (IsViewable) {
                Assert.Operation.Message( $"View {View} must be disposed" ).Valid( View.IsDisposed );
            }
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
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.AddChild( child, argument );
        }
        protected override void RemoveChild(UIWidgetBase child, object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.RemoveChild( child, argument );
            child.Dispose();
        }

    }
    public abstract class UIWidgetBase<TView> : UIWidgetBase where TView : notnull, UIViewBase {

        // View
        public sealed override bool IsViewable => true;
        protected internal sealed override UIViewBase? View => View2;
        protected internal TView View2 { get; init; } = default!;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // ShowSelf
        protected virtual void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-root" ).NotDisposed( !IsRoot );
            Assert.Operation.Message( $"Widget {Parent} must be viewable" ).NotDisposed( Parent.IsViewable );
            Parent.View.AddViewRecursive( View2 );
        }
        protected virtual void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-root" ).NotDisposed( !IsRoot );
            Assert.Operation.Message( $"Widget {Parent} must be viewable" ).NotDisposed( Parent.IsViewable );
            Parent.View.RemoveViewRecursive( View2 );
        }

    }
}

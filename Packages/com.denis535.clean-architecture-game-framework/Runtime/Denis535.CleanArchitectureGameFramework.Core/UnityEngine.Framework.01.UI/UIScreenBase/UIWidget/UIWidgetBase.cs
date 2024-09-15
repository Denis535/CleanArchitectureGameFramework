#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using UnityEngine;

    public abstract class UIWidgetBase : NodeBase<UIWidgetBase>, IDisposable {

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
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => this is IUIViewableWidget;
        protected internal UIViewBase? View => (this as IUIViewableWidget)?.View;

        // Constructor
        public UIWidgetBase() {
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be deactivating or inactive" ).Valid( State is State_.Deactivating or State_.Inactive );
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            if (IsViewable) {
                Assert.Operation.Message( $"View {View} must be disposed" ).Valid( View.IsDisposed );
            }
            disposeCancellationTokenSource?.Cancel();
            IsDisposed = true;
        }

    }
    public abstract class UIWidgetBase<TView> : UIWidgetBase, IUIViewableWidget where TView : notnull, UIViewBase {

        // View
        protected internal new TView View { get; init; } = default!;
        UIViewBase IUIViewableWidget.View => View;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // ShowSelf
        protected void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be activating" ).Valid( State is State_.Activating );
            Parent!.View!.AddViewRecursive( View );
        }
        protected void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be deactivating" ).Valid( State is State_.Deactivating );
            Parent!.View!.RemoveViewRecursive( View );
        }

    }
}

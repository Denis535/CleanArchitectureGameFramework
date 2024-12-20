#nullable enable
namespace UnityEngine.Framework {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.TreeMachine;
    using UnityEngine;

    public abstract class WidgetBase : NodeBase3<WidgetBase>, IDisposable {

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
        protected ScreenBase? Screen {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                Assert.Operation.Message( $"Widget {this} must be active or activating or deactivating" ).Valid( Activity is Activity_.Active or Activity_.Activating or Activity_.Deactivating );
                return (ScreenBase?) Tree;
            }
        }
        // View
        [MemberNotNullWhen( true, "ViewBase", "View" )]
        public virtual bool IsViewable {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return false;
            }
        }
        private protected virtual ViewBase? ViewBase {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return null;
            }
        }
        protected internal virtual ViewBase? View {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return ViewBase;
            }
        }

        // Constructor
        public WidgetBase() {
        }
        ~WidgetBase() {
#if DEBUG
            if (!IsDisposed) {
                Debug.LogWarning( $"Widget '{this}' must be disposed" );
            }
#endif
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
        protected override void OnBeforeAttach(object? argument) {
            base.OnBeforeAttach( argument );
        }
        protected override void OnAttach(object? argument) {
        }
        protected override void OnAfterAttach(object? argument) {
            base.OnAfterAttach( argument );
        }
        protected override void OnBeforeDetach(object? argument) {
            base.OnBeforeDetach( argument );
        }
        protected override void OnDetach(object? argument) {
        }
        protected override void OnAfterDetach(object? argument) {
            base.OnAfterDetach( argument );
        }

        // OnActivate
        protected override void OnBeforeActivate(object? argument) {
            base.OnBeforeActivate( argument );
        }
        //protected override void OnActivate(object? argument) {
        //}
        protected override void OnAfterActivate(object? argument) {
            base.OnAfterActivate( argument );
        }
        protected override void OnBeforeDeactivate(object? argument) {
            base.OnBeforeDeactivate( argument );
        }
        //protected override void OnDeactivate(object? argument) {
        //}
        protected override void OnAfterDeactivate(object? argument) {
            base.OnAfterDeactivate( argument );
        }

        // OnDescendantAttach
        protected override void OnBeforeDescendantAttach(WidgetBase descendant, object? argument) {
        }
        protected override void OnAfterDescendantAttach(WidgetBase descendant, object? argument) {
        }
        protected override void OnBeforeDescendantDetach(WidgetBase descendant, object? argument) {
        }
        protected override void OnAfterDescendantDetach(WidgetBase descendant, object? argument) {
        }

        // OnDescendantActivate
        protected override void OnBeforeDescendantActivate(WidgetBase descendant, object? argument) {
        }
        protected override void OnAfterDescendantActivate(WidgetBase descendant, object? argument) {
        }
        protected override void OnBeforeDescendantDeactivate(WidgetBase descendant, object? argument) {
        }
        protected override void OnAfterDescendantDeactivate(WidgetBase descendant, object? argument) {
        }

        // AddChild
        protected override void AddChild(WidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-disposed" ).Valid( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.AddChild( child, argument );
        }
        protected override void RemoveChild(WidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-disposed" ).Valid( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            base.RemoveChild( child, argument );
            child.Dispose();
        }

        // ShowView
        protected internal virtual void ShowView(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var wasShown = TryShowView( view );
            Assert.Operation.Message( $"View {view} was not shown" ).Valid( wasShown );
        }
        protected internal virtual void HideView(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var wasHidden = TryHideView( view );
            Assert.Operation.Message( $"View {view} was not hidden" ).Valid( wasHidden );
        }

        // ShowViewRecursive
        protected internal virtual void ShowViewRecursive(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var wasShown = TryShowViewRecursive( view );
            Assert.Operation.Message( $"View {view} was not shown" ).Valid( wasShown );
        }
        protected internal virtual void HideViewRecursive(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var wasHidden = TryHideViewRecursive( view );
            Assert.Operation.Message( $"View {view} was not hidden" ).Valid( wasHidden );
        }

        // TryShowView
        private bool TryShowView(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (IsViewable && View.TryAddView( view )) {
                return true;
            }
            return false;
        }
        private bool TryHideView(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (IsViewable && View.TryRemoveView( view )) {
                return true;
            }
            return false;
        }

        // TryShowViewRecursive
        private bool TryShowViewRecursive(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-attached to parent" ).Valid( !view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (IsViewable && View.TryAddView( view )) {
                return true;
            }
            return Parent?.TryShowViewRecursive( view ) ?? false;
        }
        private bool TryHideViewRecursive(ViewBase view) {
            Assert.Argument.Message( $"Argument 'view' ({view}) must be non-disposed" ).Valid( !view.IsDisposed );
            Assert.Argument.Message( $"Argument 'view' ({view}) must be attached to parent" ).Valid( view.IsAttachedToParent );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (IsViewable && View.TryRemoveView( view )) {
                return true;
            }
            return Parent?.TryHideViewRecursive( view ) ?? false;
        }

    }
    public abstract class WidgetBase<TView> : WidgetBase where TView : notnull, ViewBase {

        private TView view = default!;

        // View
        public sealed override bool IsViewable {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return true;
            }
        }
        private protected sealed override ViewBase? ViewBase {
            get {
                Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
                return View;
            }
        }
        protected internal new TView View {
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
        public WidgetBase() {
        }
        public override void Dispose() {
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            Assert.Operation.Message( $"Widget {this} must be released" ).Valid( View.IsDisposed );
            base.Dispose();
        }

        // ShowSelf
        protected virtual void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-root" ).NotDisposed( !IsRoot );
            Parent.ShowViewRecursive( View );
        }
        protected virtual void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-root" ).NotDisposed( !IsRoot );
            Parent.HideViewRecursive( View );
        }

    }
}

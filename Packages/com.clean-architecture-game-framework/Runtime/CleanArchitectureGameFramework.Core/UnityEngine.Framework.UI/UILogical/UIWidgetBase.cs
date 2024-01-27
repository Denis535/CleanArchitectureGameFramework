#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public abstract class UIWidgetBase : IUILogicalElement, IDisposable {

        private CancellationTokenSource? disposeCancellationTokenSource;

        // System
        private Lock Lock { get; } = new Lock();
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
        public virtual bool DisposeAutomatically => true;
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => this is IUIViewable;
        public UIViewBase? View => (this as IUIViewable)?.View;
        // Screen
        public UIWidgetState State { get; private set; } = UIWidgetState.Unattached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttached => State is UIWidgetState.Attached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttaching => State is UIWidgetState.Attaching;
        [MemberNotNullWhen( true, "Screen" )] public bool IsDetaching => State is UIWidgetState.Detaching;
        [MemberNotNullWhen( false, "Screen" )] public bool IsNonAttached => State is UIWidgetState.Unattached or UIWidgetState.Detached;
        public UIScreenBase? Screen { get; private set; }
        // Parent
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase? Parent { get; internal set; }
        public IReadOnlyList<UIWidgetBase> Ancestors => this.GetAncestors();
        public IReadOnlyList<UIWidgetBase> AncestorsAndSelf => this.GetAncestorsAndSelf();
        // Children
        public bool HasChildren => Children_.Any();
        private List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        public IReadOnlyList<UIWidgetBase> Descendants => this.GetDescendants();
        public IReadOnlyList<UIWidgetBase> DescendantsAndSelf => this.GetDescendantsAndSelf();
        // OnAttach
        public event Action? OnBeforeAttachEvent;
        public event Action? OnAfterAttachEvent;
        public event Action? OnBeforeDetachEvent;
        public event Action? OnAfterDetachEvent;
        // OnDescendantAttach
        public event Action<UIWidgetBase>? OnBeforeDescendantAttachEvent;
        public event Action<UIWidgetBase>? OnAfterDescendantAttachEvent;
        public event Action<UIWidgetBase>? OnBeforeDescendantDetachEvent;
        public event Action<UIWidgetBase>? OnAfterDescendantDetachEvent;

        // Constructor
        public UIWidgetBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"Widget {this} must be alive" ).Alive( !IsDisposed );
            Assert.Object.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            foreach (var child in Children) {
                if (child.DisposeAutomatically) {
                    child.Dispose();
                }
            }
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }
         
        // OnAttach
        public virtual void OnBeforeAttach() {
            OnBeforeAttachEvent?.Invoke();
            Parent?.OnBeforeDescendantAttach( this );
        }
        public abstract void OnAttach();
        public virtual void OnAfterAttach() {
            OnAfterAttachEvent?.Invoke();
            Parent?.OnAfterDescendantAttach( this );
        }
        public virtual void OnBeforeDetach() {
            OnBeforeDetachEvent?.Invoke();
            Parent?.OnBeforeDescendantDetach( this );
        }
        public abstract void OnDetach();
        public virtual void OnAfterDetach() {
            OnAfterDetachEvent?.Invoke();
            Parent?.OnAfterDescendantDetach( this );
        }

        // AttachChild
        protected internal virtual void __AttachChild__(UIWidgetBase child) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Object.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            using (Lock.Enter()) {
                Children_.Add( child );
                child.Parent = this;
                if (IsAttached) {
                    AttachToScreen( child, Screen );
                }
            }
        }
        protected internal virtual void __DetachChild__(UIWidgetBase child) {
            // You can override it but you should not directly call this method
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Object.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            using (Lock.Enter()) {
                if (IsAttached) {
                    DetachFromScreen( child, Screen );
                }
                child.Parent = null;
                Children_.Remove( child );
            }
            if (child.DisposeAutomatically) {
                child.Dispose();
            }
        }

        // OnDescendantAttach
        public virtual void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            OnBeforeDescendantAttachEvent?.Invoke( this );
            Parent?.OnBeforeDescendantAttach( descendant );
        }
        public virtual void OnAfterDescendantAttach(UIWidgetBase descendant) {
            OnAfterDescendantAttachEvent?.Invoke( descendant );
            Parent?.OnAfterDescendantAttach( descendant );
        }
        public virtual void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            OnBeforeDescendantDetachEvent?.Invoke( descendant );
            Parent?.OnBeforeDescendantDetach( descendant );
        }
        public virtual void OnAfterDescendantDetach(UIWidgetBase descendant) {
            OnAfterDescendantDetachEvent?.Invoke( descendant );
            Parent?.OnAfterDescendantDetach( descendant );
        }

        // Helpers
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal static void AttachToScreen(UIWidgetBase widget, UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be non-attached" ).Valid( widget.IsNonAttached );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen == null );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen is not null );
            widget.State = UIWidgetState.Attaching;
            widget.Screen = screen;
            widget.OnBeforeAttach();
            {
                widget.OnAttach();
                foreach (var child in widget.Children) {
                    AttachToScreen( child, screen );
                }
            }
            widget.OnAfterAttach();
            widget.State = UIWidgetState.Attached;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal static void DetachFromScreen(UIWidgetBase widget, UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be attached" ).Valid( widget.IsAttached );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen == screen );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen is not null );
            widget.State = UIWidgetState.Detaching;
            widget.OnBeforeDetach();
            {
                foreach (var child in widget.Children.Reverse()) {
                    DetachFromScreen( child, screen );
                }
                widget.OnDetach();
            }
            widget.OnAfterDetach();
            widget.Screen = null;
            widget.State = UIWidgetState.Detached;
        }

    }
    public abstract class UIWidgetBase<TView> : UIWidgetBase, IUIViewable where TView : notnull, UIViewBase {

        // View
        public abstract new TView View { get; }
        UIViewBase IUIViewable.View => View;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            Assert.Object.Message( $"Widget {this} must be alive" ).Alive( !IsDisposed );
            Assert.Object.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            View.Dispose();
            base.Dispose();
        }

        // OnAttach
        public override void OnBeforeAttach() {
            base.OnBeforeAttach();
        }
        public override void OnAfterAttach() {
            base.OnAfterAttach();
        }
        public override void OnBeforeDetach() {
            base.OnBeforeDetach();
        }
        public override void OnAfterDetach() {
            base.OnAfterDetach();
        }

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child) {
            base.__AttachChild__( child );
        }
        protected internal override void __DetachChild__(UIWidgetBase child) {
            base.__DetachChild__( child );
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            base.OnAfterDescendantDetach( descendant );
        }

    }
}

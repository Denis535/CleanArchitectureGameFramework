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
        public Action? OnBeforeAttachEvent { get; set; }
        public Action? OnAfterAttachEvent { get; set; }
        public Action? OnBeforeDetachEvent { get; set; }
        public Action? OnAfterDetachEvent { get; set; }
        // OnDescendantAttach
        public Action<UIWidgetBase>? OnBeforeDescendantAttachEvent { get; set; }
        public Action<UIWidgetBase>? OnAfterDescendantAttachEvent { get; set; }
        public Action<UIWidgetBase>? OnBeforeDescendantDetachEvent { get; set; }
        public Action<UIWidgetBase>? OnAfterDescendantDetachEvent { get; set; }

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
        public abstract void OnAttach();
        public abstract void OnDetach();

        // OnAttach
        private void OnBeforeAttach() {
            foreach (var ancestor in this.GetAncestors().AsEnumerable().Reverse()) {
                ancestor.OnBeforeDescendantAttachEvent?.Invoke( this );
                ancestor.OnBeforeDescendantAttach( this );
            }
            OnBeforeAttachEvent?.Invoke();
        }
        private void OnAfterAttach() {
            OnAfterAttachEvent?.Invoke();
            foreach (var ancestor in this.GetAncestors()) {
                ancestor.OnAfterDescendantAttach( this );
                ancestor.OnAfterDescendantAttachEvent?.Invoke( this );
            }
        }
        private void OnBeforeDetach() {
            foreach (var ancestor in this.GetAncestors().AsEnumerable().Reverse()) {
                ancestor.OnBeforeDescendantDetachEvent?.Invoke( this );
                ancestor.OnBeforeDescendantDetach( this );
            }
            OnBeforeDetachEvent?.Invoke();
        }
        private void OnAfterDetach() {
            OnAfterDetachEvent?.Invoke();
            foreach (var ancestor in this.GetAncestors()) {
                ancestor.OnAfterDescendantDetach( this );
                ancestor.OnAfterDescendantDetachEvent?.Invoke( this );
            }
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
        public virtual void OnBeforeDescendantAttach(UIWidgetBase widget) {
        }
        public virtual void OnAfterDescendantAttach(UIWidgetBase widget) {
        }
        public virtual void OnBeforeDescendantDetach(UIWidgetBase widget) {
        }
        public virtual void OnAfterDescendantDetach(UIWidgetBase widget) {
        }

        // Helpers
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal static void AttachToScreen(UIWidgetBase widget, UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'widget' {widget} must be non-attached" ).Valid( widget.IsNonAttached );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen == null );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen );
            widget.State = UIWidgetState.Attaching;
            widget.Screen = screen;
            {
                widget.OnBeforeAttach();
                widget.OnAttach();
                foreach (var child in widget.Children) {
                    AttachToScreen( child, screen );
                }
                widget.OnAfterAttach();
            }
            widget.State = UIWidgetState.Attached;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        internal static void DetachFromScreen(UIWidgetBase widget, UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'widget' {widget} must be attached" ).Valid( widget.IsAttached );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen != null );
            Assert.Argument.Message( $"Argument 'widget' {widget} must be valid" ).Valid( widget.Screen == screen );
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen );
            widget.State = UIWidgetState.Detaching;
            {
                widget.OnBeforeDetach();
                foreach (var child in widget.Children.Reverse()) {
                    DetachFromScreen( child, screen );
                }
                widget.OnDetach();
                widget.OnAfterDetach();
            }
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

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child) {
            base.__AttachChild__( child );
        }
        protected internal override void __DetachChild__(UIWidgetBase child) {
            base.__DetachChild__( child );
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase widget) {
            base.OnBeforeDescendantAttach( widget );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase widget) {
            base.OnAfterDescendantAttach( widget );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase widget) {
            base.OnBeforeDescendantDetach( widget );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase widget) {
            widget.OnAfterDescendantDetach( widget );
        }

    }
}

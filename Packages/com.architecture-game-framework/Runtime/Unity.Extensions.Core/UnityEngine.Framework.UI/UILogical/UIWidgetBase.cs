#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
        // Owner
        internal IUILogicalElement? Owner => (IUILogicalElement?) Parent ?? Screen;
        // Screen
        public UIWidgetState State { get; private set; } = UIWidgetState.Unattached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttached => State is UIWidgetState.Attached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttaching => State is UIWidgetState.Attaching;
        [MemberNotNullWhen( true, "Screen" )] public bool IsDetaching => State is UIWidgetState.Detaching;
        [MemberNotNullWhen( false, "Screen" )] public bool IsNonAttached => State is UIWidgetState.Unattached or UIWidgetState.Detached;
        public UIScreenBase? Screen { get; private set; }
        // Parent
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase? Parent { get; private set; }
        public IReadOnlyList<UIWidgetBase> Ancestors => this.GetAncestors();
        public IReadOnlyList<UIWidgetBase> AncestorsAndSelf => this.GetAncestorsAndSelf();
        // Children
        public bool HasChildren => Children_.Any();
        private List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        public IReadOnlyList<UIWidgetBase> Descendants => this.GetDescendants();
        public IReadOnlyList<UIWidgetBase> DescendantsAndSelf => this.GetDescendantsAndSelf();
        // OnAttach
        //public Action? OnBeforeAttachEvent { get; set; }
        //public Action? OnAttachEvent { get; set; }
        //public Action? OnDetachEvent { get; set; }
        //public Action? OnAfterDetachEvent { get; set; }
        // OnDescendantAttach
        //public Action<UIWidgetBase>? OnBeforeDescendantAttachEvent { get; set; }
        //public Action<UIWidgetBase>? OnAfterDescendantAttachEvent { get; set; }
        //public Action<UIWidgetBase>? OnBeforeDescendantDetachEvent { get; set; }
        //public Action<UIWidgetBase>? OnAfterDescendantDetachEvent { get; set; }

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

        // Attach
        internal void Attach(UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen );
            Assert.Object.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen == null );
            State = UIWidgetState.Attaching;
            Screen = screen;
            //Parent?.OnBeforeDescendantAttachEvent?.Invoke( this );
            //Parent?.OnBeforeDescendantAttach( this );
            {
                // OnBeforeAttach
                //OnBeforeAttachEvent?.Invoke();
                //OnBeforeAttach();
                // OnAttach
                //Screen!.ShowWidget( this );
                //OnAttachEvent?.Invoke();
                //OnAttach();
            }
            foreach (var child in Children) {
                child.Attach( Screen );
            }
            //Parent?.OnAfterDescendantAttach( this );
            //Parent?.OnAfterDescendantAttachEvent?.Invoke( this );
            State = UIWidgetState.Attached;
        }
        internal void Detach(UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen );
            Assert.Object.Message( $"Widget {this} must be attached" ).Valid( IsAttached );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen != null );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen == screen );
            State = UIWidgetState.Detaching;
            //Parent?.OnBeforeDescendantDetachEvent?.Invoke( this );
            //Parent?.OnBeforeDescendantDetach( this );
            foreach (var child in Children.Reverse()) {
                child.Detach( Screen );
            }
            {
                // OnDetach
                //OnDetach();
                //OnDetachEvent?.Invoke();
                //Screen!.HideWidget( this );
                // OnAfterDetach
                //OnAfterDetach();
                //OnAfterDetachEvent?.Invoke();
            }
            //Parent?.OnAfterDescendantDetachEvent?.Invoke( this );
            //Parent?.OnAfterDescendantDetach( this );
            Screen = null;
            State = UIWidgetState.Detached;
        }

        // Attach
        private void Attach(UIWidgetBase parent) {
            Assert.Argument.Message( $"Argument 'parent' must be non-null" ).NotNull( parent != null );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Parent == null );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( IsNonAttached );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen == null );
            Parent = parent;
            if (Parent.IsAttached) {
                Attach( Parent.Screen );
            }
        }
        private void Detach(UIWidgetBase parent) {
            Assert.Argument.Message( $"Argument 'parent' must be non-null" ).NotNull( parent != null );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Parent != null );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Parent == parent );
            if (Parent.IsAttached) {
                //Assert.Object.Message( $"Widget {this} must be valid" ).Valid( IsAttached );
                //Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen != null );
                //Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen == Parent.Screen );
                Detach( Parent.Screen );
            }
            Parent = null;
        }

        // OnAttach
        //public abstract void OnBeforeAttach();
        //public abstract void OnAttach();
        //public abstract void OnDetach();
        //public abstract void OnAfterDetach();

        // AttachChild
        protected internal virtual void __AttachChild__(UIWidgetBase child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Object.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            using (Lock.Enter()) {
                Children_.Add( child );
                child.Attach( this );
            }
        }
        protected internal virtual void __DetachChild__(UIWidgetBase child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Object.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            using (Lock.Enter()) {
                child.Detach( this );
                Children_.Remove( child );
            }
            if (child.DisposeAutomatically) {
                child.Dispose();
            }
        }

        // OnDescendantAttach
        //public virtual void OnBeforeDescendantAttach(UIWidgetBase descendant) {
        //    Parent?.OnBeforeDescendantAttachEvent?.Invoke( descendant );
        //    Parent?.OnBeforeDescendantAttach( descendant );
        //}
        //public virtual void OnAfterDescendantAttach(UIWidgetBase descendant) {
        //    Parent?.OnAfterDescendantAttach( descendant );
        //    Parent?.OnAfterDescendantAttachEvent?.Invoke( descendant );
        //}
        //public virtual void OnBeforeDescendantDetach(UIWidgetBase descendant) {
        //    Parent?.OnBeforeDescendantDetachEvent?.Invoke( descendant );
        //    Parent?.OnBeforeDescendantDetach( descendant );
        //}
        //public virtual void OnAfterDescendantDetach(UIWidgetBase descendant) {
        //    Parent?.OnAfterDescendantDetach( descendant );
        //    Parent?.OnAfterDescendantDetachEvent?.Invoke( descendant );
        //}

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
        //public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
        //    base.OnBeforeDescendantAttach( descendant );
        //}
        //public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
        //    base.OnAfterDescendantAttach( descendant );
        //}
        //public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
        //    base.OnBeforeDescendantDetach( descendant );
        //}
        //public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
        //    base.OnAfterDescendantDetach( descendant );
        //}

    }
}

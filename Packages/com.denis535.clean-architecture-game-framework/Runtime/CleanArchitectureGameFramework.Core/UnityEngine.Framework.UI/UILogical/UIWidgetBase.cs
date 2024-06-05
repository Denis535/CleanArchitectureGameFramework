#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEngine;

    public abstract partial class UIWidgetBase : Disposable, IUILogicalElement, IDisposable {

        private readonly Lock @lock = new Lock();

        // System
        public bool DisposeWhenDetach { get; protected init; } = true;
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => this is IUIViewable;
        public UIViewBase? View => (this as IUIViewable)?.View;
        // State
        public UIWidgetState State { get; internal set; } = UIWidgetState.Unattached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttached => State is UIWidgetState.Attached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttaching => State is UIWidgetState.Attaching;
        [MemberNotNullWhen( true, "Screen" )] public bool IsDetaching => State is UIWidgetState.Detaching;
        [MemberNotNullWhen( false, "Screen" )] public bool IsNonAttached => State is UIWidgetState.Unattached or UIWidgetState.Detached;
        // Screen
        public UIScreenBase? Screen { get; internal set; }
        // Parent
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase? Parent { get; internal set; }
        // Children
        public bool HasChildren => Children_.Any();
        private List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        // OnAttach
        public event Action<object?>? OnBeforeAttachEvent;
        public event Action<object?>? OnAfterAttachEvent;
        public event Action<object?>? OnBeforeDetachEvent;
        public event Action<object?>? OnAfterDetachEvent;
        // OnDescendantAttach
        public event Action<UIWidgetBase, object?>? OnBeforeDescendantAttachEvent;
        public event Action<UIWidgetBase, object?>? OnAfterDescendantAttachEvent;
        public event Action<UIWidgetBase, object?>? OnBeforeDescendantDetachEvent;
        public event Action<UIWidgetBase, object?>? OnAfterDescendantDetachEvent;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            this.ThrowIfDisposed();
            Assert.Operation.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            foreach (var child in Children) {
                child.Dispose();
            }
#if UNITY_EDITOR
            Assert.Operation.Message( $"Widget {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
#endif
            base.Dispose();
        }

        // OnAttach
        public virtual void OnBeforeAttach(object? argument) {
            Parent?.OnBeforeDescendantAttach( this, argument );
            OnBeforeAttachEvent?.Invoke( argument );
            // override here
        }
        public abstract void OnAttach(object? argument); // override to init and show self
        public virtual void OnAfterAttach(object? argument) {
            // override here
            OnAfterAttachEvent?.Invoke( argument );
            Parent?.OnAfterDescendantAttach( this, argument );
        }

        // OnDetach
        public virtual void OnBeforeDetach(object? argument) {
            Parent?.OnBeforeDescendantDetach( this, argument );
            OnBeforeDetachEvent?.Invoke( argument );
            // override here
        }
        public abstract void OnDetach(object? argument); // override to hide self and deinit
        public virtual void OnAfterDetach(object? argument) {
            // override here
            OnAfterDetachEvent?.Invoke( argument );
            Parent?.OnAfterDescendantDetach( this, argument );
        }

        // OnDescendantAttach
        public virtual void OnBeforeDescendantAttach(UIWidgetBase descendant, object? argument) {
            Parent?.OnBeforeDescendantAttach( descendant, argument );
            OnBeforeDescendantAttachEvent?.Invoke( descendant, argument );
            // override here
        }
        public virtual void OnAfterDescendantAttach(UIWidgetBase descendant, object? argument) {
            // override here
            OnAfterDescendantAttachEvent?.Invoke( descendant, argument );
            Parent?.OnAfterDescendantAttach( descendant, argument );
        }

        // OnDescendantDetach
        public virtual void OnBeforeDescendantDetach(UIWidgetBase descendant, object? argument) {
            Parent?.OnBeforeDescendantDetach( descendant, argument );
            OnBeforeDescendantDetachEvent?.Invoke( descendant, argument );
            // override here
        }
        public virtual void OnAfterDescendantDetach(UIWidgetBase descendant, object? argument) {
            // override here
            OnAfterDescendantDetachEvent?.Invoke( descendant, argument );
            Parent?.OnAfterDescendantDetach( descendant, argument );
        }

        // AttachChild
        public virtual void AttachChild(UIWidgetBase child, object? argument = null) {
            // You can override it but you should not directly call this method
            this.ThrowIfDisposed();
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            using (@lock.Enter()) {
                Children_.Add( child );
                child.Parent = this;
                if (IsAttached) {
                    child.AttachToScreen( Screen, argument );
                } else {
                    Assert.Operation.Message( $"You are trying to attach child {child} with argument {argument}, but widget {this} must be attached" ).Valid( argument == null );
                }
            }
        }
        public virtual void DetachChild(UIWidgetBase child, object? argument = null) {
            // You can override it but you should not directly call this method
            this.ThrowIfDisposed();
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            using (@lock.Enter()) {
                if (IsAttached) {
                    child.DetachFromScreen( Screen, argument );
                } else {
                    Assert.Operation.Message( $"You are trying to detach child {child} with argument {argument}, but widget {this} must be attached" ).Valid( argument == null );
                }
                child.Parent = null;
                Children_.Remove( child );
            }
            if (child.DisposeWhenDetach) {
                child.Dispose();
            }
        }

    }
    public abstract partial class UIWidgetBase {

        // ShowSelf
        public void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be attaching" ).Valid( IsAttaching );
            Assert.Operation.Message( $"Widget {this} must be non-shown" ).Valid( View.VisualElement.parent == null );
            Parent!.ShowView( View );
            Assert.Operation.Message( $"Widget {this} was not shown" ).Valid( View.VisualElement.parent != null );
        }
        public void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be detaching" ).Valid( IsDetaching );
            Assert.Operation.Message( $"Widget {this} must be shown" ).Valid( View.VisualElement.parent != null );
            Parent!.HideView( View );
            Assert.Operation.Message( $"Widget {this} was not hidden" ).Valid( View.VisualElement.parent == null );
        }

        // ShowView
        public virtual void ShowView(UIViewBase view) {
            // override here
            Assert.Operation.Message( $"View {view} must be non-shown" ).Valid( view.VisualElement.parent == null );
            Parent!.ShowView( view );
            Assert.Operation.Message( $"View {view} was not shown" ).Valid( view.VisualElement.parent != null );
        }
        public virtual void HideView(UIViewBase view) {
            // override here
            Assert.Operation.Message( $"View {view} must be shown" ).Valid( view.VisualElement.parent != null );
            Parent!.HideView( view );
            Assert.Operation.Message( $"View {view} was not hidden" ).Valid( view.VisualElement.parent == null );
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
            this.ThrowIfDisposed();
            Assert.Operation.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            foreach (var child in Children) {
                child.Dispose();
            }
            Assert.Operation.Message( $"Widget {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            View.Dispose();
            Dispose( this );
        }

    }
}

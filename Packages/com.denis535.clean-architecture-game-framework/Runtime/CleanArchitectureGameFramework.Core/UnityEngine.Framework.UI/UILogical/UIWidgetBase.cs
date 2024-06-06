#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEngine;

    public abstract partial class UIWidgetBase : Disposable, IUILogicalElement, IDisposable {

        internal readonly Lock @lock = new Lock();

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
        internal UIScreenBase? Screen { get; set; }
        // Parent
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase? Parent { get; internal set; }
        // Children
        public bool HasChildren => Children_.Any();
        internal List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
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
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            Children.DisposeAll();
            base.Dispose();
        }

    }
    public abstract partial class UIWidgetBase {

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

        // AddChild
        public virtual void AddChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).NotNull( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            this.AddChildInternal( child, argument );
        }

        // RemoveChild
        public virtual void RemoveChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).NotNull( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            this.RemoveChildInternal( child, argument );
        }
        public void RemoveChild<T>(object? argument = null) where T : UIWidgetBase {
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have child {typeof( T )} widget" ).Valid( Children.OfType<T>().Any() );
            this.RemoveChildInternal( Children.OfType<T>().Last(), argument );
        }

        // RemoveChildren
        public void RemoveChildren(object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            foreach (var child in Children.Reverse()) {
                RemoveChild( child, argument );
            }
        }

        // RemoveSelf
        public void RemoveSelf(object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have parent or screen" ).Valid( Parent != null || Screen != null );
            if (Parent != null) {
                Parent.RemoveChild( this, argument );
            } else {
                Screen!.RemoveWidget( this, argument );
            }
        }

    }
    public abstract partial class UIWidgetBase {

        // ShowSelf
        public void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be attaching" ).Valid( IsAttaching );
            Assert.Operation.Message( $"Widget {this} must be hidden" ).Valid( View.VisualElement.parent == null );
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
            Assert.Operation.Message( $"Widget {this} must not be disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            Children.DisposeAll();
            View.Dispose();
            DisposeInternal();
        }

    }
}

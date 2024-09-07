#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEngine;

    public abstract class UIWidgetBase : Disposable {

        // System
        protected virtual bool DisposeWhenDeactivate => true;
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => this is IUIViewableWidget;
        protected internal UIViewBase? View => (this as IUIViewableWidget)?.View;
        // State
        public UIWidgetState State { get; private set; } = UIWidgetState.Inactive;
        // Owner
        private object? Owner { get; set; }
        // Screen
        public UIScreenBase? Screen => Owner as UIScreenBase;
        // Parent
        public UIWidgetBase? Parent => Owner as UIWidgetBase;
        // Root
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase Root => IsRoot ? this : Parent.Root;
        // Ancestors
        public IEnumerable<UIWidgetBase> Ancestors {
            get {
                if (Parent != null) {
                    yield return Parent;
                    foreach (var i in Parent.Ancestors) yield return i;
                }
            }
        }
        public IEnumerable<UIWidgetBase> AncestorsAndSelf => Ancestors.Prepend( this );
        // Children
        private List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        // Descendants
        public IEnumerable<UIWidgetBase> Descendants {
            get {
                foreach (var child in Children) {
                    yield return child;
                    foreach (var i in child.Descendants) yield return i;
                }
            }
        }
        public IEnumerable<UIWidgetBase> DescendantsAndSelf => Descendants.Prepend( this );
        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;
        // OnDescendantActivate
        public event Action<UIWidgetBase, object?>? OnBeforeDescendantActivateEvent;
        public event Action<UIWidgetBase, object?>? OnAfterDescendantActivateEvent;
        public event Action<UIWidgetBase, object?>? OnBeforeDescendantDeactivateEvent;
        public event Action<UIWidgetBase, object?>? OnAfterDescendantDeactivateEvent;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( State is UIWidgetState.Inactive );
            if (IsViewable) {
                Assert.Operation.Message( $"View {View} must be disposed" ).Valid( View.IsDisposed );
            }
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            base.Dispose();
        }

        // Activate
        internal void Activate(UIScreenBase owner, object? argument) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Owner = owner;
            Activate( argument );
        }
        internal void Deactivate(UIScreenBase owner, object? argument) {
            Assert.Argument.Message( $"Argument 'owner' ({owner}) must be valid" ).Valid( owner == Owner );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Deactivate( argument );
            Owner = null;
        }

        // Activate
        internal void Activate(UIWidgetBase owner, object? argument) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (owner.State is UIWidgetState.Active) {
                Owner = owner;
                Activate( argument );
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Owner = owner;
            }
        }
        internal void Deactivate(UIWidgetBase owner, object? argument) {
            Assert.Argument.Message( $"Argument 'owner' ({owner}) must be valid" ).Valid( owner == Owner );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            if (owner.State is UIWidgetState.Active) {
                Deactivate( argument );
                Owner = null;
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Owner = null;
            }
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( State is UIWidgetState.Inactive );
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( this, argument );
                ancestor.OnBeforeDescendantActivate( this, argument );
            }
            {
                OnBeforeActivateEvent?.Invoke( argument );
                OnBeforeActivate( argument );
                State = UIWidgetState.Activating;
                {
                    OnActivate( argument );
                    foreach (var child in Children) {
                        child.Activate( argument );
                    }
                }
                State = UIWidgetState.Active;
                OnAfterActivate( argument );
                OnAfterActivateEvent?.Invoke( argument );
            }
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( this, argument );
            }
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be active" ).Valid( State is UIWidgetState.Active );
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( this, argument );
                ancestor.OnBeforeDescendantDeactivate( this, argument );
            }
            {
                OnBeforeDeactivateEvent?.Invoke( argument );
                OnBeforeDeactivate( argument );
                State = UIWidgetState.Deactivating;
                {
                    foreach (var child in Children.Reverse()) {
                        child.Deactivate( argument );
                    }
                    OnDeactivate( argument );
                }
                State = UIWidgetState.Inactive;
                OnAfterDeactivate( argument );
                OnAfterDeactivateEvent?.Invoke( argument );
            }
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDeactivate( this, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( this, argument );
            }
            if (DisposeWhenDeactivate) {
                Dispose();
            }
        }

        // OnActivate
        protected virtual void OnBeforeActivate(object? argument) {
        }
        protected abstract void OnActivate(object? argument); // override to init and show self
        protected virtual void OnAfterActivate(object? argument) {
        }
        protected virtual void OnBeforeDeactivate(object? argument) {
        }
        protected abstract void OnDeactivate(object? argument); // override to hide self and deinit
        protected virtual void OnAfterDeactivate(object? argument) {
        }

        // OnDescendantActivate
        protected abstract void OnBeforeDescendantActivate(UIWidgetBase descendant, object? argument);
        protected abstract void OnAfterDescendantActivate(UIWidgetBase descendant, object? argument);
        protected abstract void OnBeforeDescendantDeactivate(UIWidgetBase descendant, object? argument);
        protected abstract void OnAfterDescendantDeactivate(UIWidgetBase descendant, object? argument);

        // AddChild
        public virtual void AddChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-disposed" ).Valid( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            Children_.Add( child );
            Sort( Children_ );
            child.Activate( this, argument );
        }
        public virtual void RemoveChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-disposed" ).Valid( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            child.Deactivate( this, argument );
            Children_.Remove( child );
        }
        public bool RemoveChild(Func<UIWidgetBase, bool> predicate, object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var child = Children.LastOrDefault( predicate );
            if (child != null) {
                RemoveChild( child, argument );
                return true;
            }
            return false;
        }
        public void RemoveChildren(IEnumerable<UIWidgetBase> children, object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            foreach (var child in children) {
                RemoveChild( child, argument );
            }
        }
        public int RemoveChildren(Func<UIWidgetBase, bool> predicate, object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var children = Children.Where( predicate ).Reverse().ToList();
            if (children.Any()) {
                RemoveChildren( children, argument );
                return children.Count;
            }
            return 0;
        }
        public void RemoveSelf(object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have owner" ).Valid( Owner != null );
            if (Owner is UIWidgetBase parent) {
                parent.RemoveChild( this, argument );
            } else {
                ((UIScreenBase) Owner).RemoveWidget( this, argument );
            }
        }

        // Sort
        protected virtual void Sort(List<UIWidgetBase> children) {
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
            Assert.Operation.Message( $"Widget {this} must be activating" ).Valid( State is UIWidgetState.Activating );
            Parent!.View!.AddViewRecursive( View );
        }
        protected void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be deactivating" ).Valid( State is UIWidgetState.Deactivating );
            Parent!.View!.RemoveViewRecursive( View );
        }

    }
}

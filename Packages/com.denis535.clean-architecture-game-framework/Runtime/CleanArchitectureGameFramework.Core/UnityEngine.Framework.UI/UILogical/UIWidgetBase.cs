#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEngine;

    public abstract partial class UIWidgetBase : Disposable, IDisposable {

        // System
        protected virtual bool DisposeWhenDeactivate => true;
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => this is IUIViewableWidget;
        public UIViewBase? View => (this as IUIViewableWidget)?.View;
        // State
        public UIWidgetState State { get; private set; } = UIWidgetState.Inactive;
        // Screen
        private UIScreenBase? Screen { get; set; }
        // Root
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase Root => IsRoot ? this : Parent.Root;
        // Parent
        public UIWidgetBase? Parent { get; private set; }
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
        public bool HasChildren => Children_.Any();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        private List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
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
            Assert.Operation.Message( $"Widget {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            base.Dispose();
        }

        // Activate
        internal void Activate(UIScreenBase screen, object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( this, argument );
                ancestor.OnBeforeDescendantActivate( this, argument );
            }
            {
                OnBeforeActivateEvent?.Invoke( argument );
                OnBeforeActivate( argument );
                {
                    State = UIWidgetState.Activating;
                    Screen = screen;
                    OnActivate( argument );
                    foreach (var child in Children) {
                        child.Activate( screen, argument );
                    }
                    State = UIWidgetState.Active;
                }
                OnAfterActivate( argument );
                OnAfterActivateEvent?.Invoke( argument );
            }
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( this, argument );
            }
        }
        internal void Deactivate(UIScreenBase screen, object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( this, argument );
                ancestor.OnBeforeDescendantDeactivate( this, argument );
            }
            {
                OnBeforeDeactivateEvent?.Invoke( argument );
                OnBeforeDeactivate( argument );
                {
                    State = UIWidgetState.Deactivating;
                    foreach (var child in Children.Reverse()) {
                        child.Deactivate( screen, argument );
                    }
                    OnDeactivate( argument );
                    Screen = null;
                    State = UIWidgetState.Inactive;
                }
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

    }
    public abstract partial class UIWidgetBase {

        // AddChild
        public virtual void AddChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).NotNull( !child.IsDisposed );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).Valid( child.State is UIWidgetState.Inactive );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            if (State is UIWidgetState.Active) {
                Children_.Add( child );
                child.Parent = this;
                child.Activate( Screen!, argument );
            } else {
                Assert.Operation.Message( $"Argument {argument} must be null" ).Valid( argument == null );
                Children_.Add( child );
                child.Parent = this;
            }
        }
        public virtual void RemoveChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).NotNull( !child.IsDisposed );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).Valid( child.State is UIWidgetState.Active );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            if (State is UIWidgetState.Active) {
                child.Deactivate( Screen!, argument );
                child.Parent = null;
                Children_.Remove( child );
            } else {
                Assert.Operation.Message( $"Argument {argument} must be null" ).Valid( argument == null );
                child.Parent = null;
                Children_.Remove( child );
            }
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
        public int RemoveChildren(Func<UIWidgetBase, bool> predicate, object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var children = Children.Where( predicate ).Reverse().ToList();
            foreach (var child in children) {
                RemoveChild( child, argument );
            }
            return children.Count;
        }
        public void RemoveSelf(object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
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
        protected void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be activating" ).Valid( State is UIWidgetState.Activating );
            Assert.Operation.Message( $"Widget {this} must be hidden" ).Valid( !View.IsShown );
            Parent!.ShowView( View );
            Assert.Operation.Message( $"Widget {this} must be shown" ).Valid( View.IsShown );
        }
        protected void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be deactivating" ).Valid( State is UIWidgetState.Deactivating );
            Assert.Operation.Message( $"Widget {this} must be shown" ).Valid( View.IsShown );
            Parent!.HideView( View );
            Assert.Operation.Message( $"Widget {this} must be hidden" ).Valid( !View.IsShown );
        }

        // ShowView
        protected virtual void ShowView(UIViewBase view) {
            // override here
            Assert.Operation.Message( $"View {view} must be hidden" ).Valid( !view.IsShown );
            Parent!.ShowView( view );
            Assert.Operation.Message( $"View {view} must be shown" ).Valid( view.IsShown );
        }
        protected virtual void HideView(UIViewBase view) {
            // override here
            Assert.Operation.Message( $"View {view} must be shown" ).Valid( view.IsShown );
            Parent!.HideView( view );
            Assert.Operation.Message( $"View {view} must be hidden" ).Valid( !view.IsShown );
        }

    }
    public abstract class UIWidgetBase<TView> : UIWidgetBase, IUIViewableWidget where TView : notnull, UIViewBase {

        // View
        public abstract new TView View { get; }
        UIViewBase IUIViewableWidget.View => View;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( State is UIWidgetState.Inactive );
            Assert.Operation.Message( $"Widget {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            View.Dispose();
            DisposeInternal();
        }

    }
}

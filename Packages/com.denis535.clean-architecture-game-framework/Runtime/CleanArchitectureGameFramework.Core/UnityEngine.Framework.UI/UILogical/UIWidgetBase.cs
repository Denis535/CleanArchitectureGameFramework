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
        public virtual bool DisposeWhenDeactivate => true;
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => this is IUIViewable;
        public UIViewBase? View => (this as IUIViewable)?.View;
        // State
        public UIWidgetState State { get; internal set; } = UIWidgetState.Inactive;
        // Screen
        internal UIScreenBase? Screen { get; set; }
        // Parent
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase? Parent { get; internal set; }
        // Children
        public bool HasChildren => Children_.Any();
        internal List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        // OnActivate
        public Action<object?>? OnBeforeActivateEvent { get; set; }
        public Action<object?>? OnAfterActivateEvent { get; set; }
        public Action<object?>? OnBeforeDeactivateEvent { get; set; }
        public Action<object?>? OnAfterDeactivateEvent { get; set; }
        // OnDescendantActivate
        public Action<UIWidgetBase, object?>? OnBeforeDescendantActivateEvent { get; set; }
        public Action<UIWidgetBase, object?>? OnAfterDescendantActivateEvent { get; set; }
        public Action<UIWidgetBase, object?>? OnBeforeDescendantDeactivateEvent { get; set; }
        public Action<UIWidgetBase, object?>? OnAfterDescendantDeactivateEvent { get; set; }

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( State is UIWidgetState.Inactive );
            Assert.Operation.Message( $"Widget {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            base.Dispose();
        }

    }
    public abstract partial class UIWidgetBase {

        // OnActivate
        public virtual void OnBeforeActivate(object? argument) {
        }
        public abstract void OnActivate(object? argument); // override to init and show self
        public virtual void OnAfterActivate(object? argument) {
        }
        public virtual void OnBeforeDeactivate(object? argument) {
        }
        public abstract void OnDeactivate(object? argument); // override to hide self and deinit
        public virtual void OnAfterDeactivate(object? argument) {
        }

        // OnDescendantActivate
        public abstract void OnBeforeDescendantActivate(UIWidgetBase descendant, object? argument);
        public abstract void OnAfterDescendantActivate(UIWidgetBase descendant, object? argument);
        public abstract void OnBeforeDescendantDeactivate(UIWidgetBase descendant, object? argument);
        public abstract void OnAfterDescendantDeactivate(UIWidgetBase descendant, object? argument);

        // AddChild
        public virtual void AddChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).NotNull( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            this.AddChildInternal( child, argument );
        }
        public virtual void RemoveChild(UIWidgetBase child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' must be valid" ).NotNull( !child.IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            this.RemoveChildInternal( child, argument );
        }
        public int RemoveChildren(Func<UIWidgetBase, bool> predicate, object? argument = null) {
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            var count = 0;
            foreach (var child in Children.Reverse().ToList()) {
                if (predicate( child )) {
                    RemoveChild( child, argument );
                    count++;
                }
            }
            return count;
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
        public void ShowSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be activating" ).Valid( State is UIWidgetState.Activating );
            Assert.Operation.Message( $"Widget {this} must be hidden" ).Valid( View.VisualElement.parent == null );
            Parent!.ShowView( View );
            Assert.Operation.Message( $"Widget {this} was not shown" ).Valid( View.VisualElement.parent != null );
        }
        public void HideSelf() {
            Assert.Operation.Message( $"Widget {this} must be viewable" ).Valid( IsViewable );
            Assert.Operation.Message( $"Widget {this} must be deactivating" ).Valid( State is UIWidgetState.Deactivating );
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
            Assert.Operation.Message( $"Widget {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"Widget {this} must be inactive" ).Valid( State is UIWidgetState.Inactive );
            Assert.Operation.Message( $"Widget {this} children must be disposed" ).Valid( Children.All( i => i.IsDisposed ) );
            View.Dispose();
            DisposeInternal();
        }

    }
}

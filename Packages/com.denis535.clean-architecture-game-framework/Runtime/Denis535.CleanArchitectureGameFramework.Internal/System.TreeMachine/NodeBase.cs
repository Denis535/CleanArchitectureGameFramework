#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public abstract class NodeBase<TThis> where TThis : NodeBase<TThis> {
        public enum Activity_ {
            Inactive,
            Activating,
            Active,
            Deactivating,
        }

        // Activity
        public Activity_ Activity { get; private set; } = Activity_.Inactive;
        // Owner
        private object? Owner { get; set; }
        // Tree
        public ITree<TThis>? Tree => Owner as ITree<TThis>;
        // Root
        [MemberNotNullWhen( false, nameof( Parent ) )] public bool IsRoot => Parent == null;
        public TThis Root => Parent?.Root ?? (TThis) this;
        // Parent
        public TThis? Parent => Owner as TThis;
        // Ancestors
        public IEnumerable<TThis> Ancestors {
            get {
                if (Parent != null) {
                    yield return Parent;
                    foreach (var i in Parent.Ancestors) yield return i;
                }
            }
        }
        public IEnumerable<TThis> AncestorsAndSelf => Ancestors.Prepend( (TThis) this );
        // Children
        private List<TThis> Children_ { get; } = new List<TThis>( 0 );
        public IReadOnlyList<TThis> Children => Children_;
        // Descendants
        public IEnumerable<TThis> Descendants {
            get {
                foreach (var child in Children) {
                    yield return child;
                    foreach (var i in child.Descendants) yield return i;
                }
            }
        }
        public IEnumerable<TThis> DescendantsAndSelf => Descendants.Prepend( (TThis) this );
        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;

        // Constructor
        public NodeBase() {
        }
        protected virtual void DisposeWhenDeactivate() {
            (this as IDisposable)?.Dispose();
        }

        // SetOwner
        internal void SetOwner(ITree<TThis> owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
            Owner = owner;
            Activate( argument );
        }
        internal void RemoveOwner(ITree<TThis> owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
            Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
            Deactivate( argument );
            Owner = null;
        }

        // SetOwner
        internal void SetOwner(TThis owner, object? argument) {
            if (owner.Activity is Activity_.Active) {
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Owner = owner;
                Activate( argument );
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Owner = owner;
            }
        }
        internal void RemoveOwner(TThis owner, object? argument) {
            if (owner.Activity is Activity_.Active) {
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Deactivate( argument );
                Owner = null;
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Inactive );
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Owner = null;
            }
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            BeforeActivate( argument );
            Activity = Activity_.Activating;
            {
                OnActivate( argument );
                foreach (var child in Children) {
                    child.Activate( argument );
                }
            }
            Activity = Activity_.Active;
            AfterActivate( argument );
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
            BeforeDeactivate( argument );
            Activity = Activity_.Deactivating;
            {
                foreach (var child in Children.Reverse()) {
                    child.Deactivate( argument );
                }
                OnDeactivate( argument );
            }
            Activity = Activity_.Inactive;
            AfterDeactivate( argument );
            DisposeWhenDeactivate();
        }

        // Activate
        private protected virtual void BeforeActivate(object? argument) {
            OnBeforeActivateEvent?.Invoke( argument );
            OnBeforeActivate( argument );
        }
        private protected virtual void AfterActivate(object? argument) {
            OnAfterActivate( argument );
            OnAfterActivateEvent?.Invoke( argument );
        }
        private protected virtual void BeforeDeactivate(object? argument) {
            OnBeforeDeactivateEvent?.Invoke( argument );
            OnBeforeDeactivate( argument );
        }
        private protected virtual void AfterDeactivate(object? argument) {
            OnAfterDeactivate( argument );
            OnAfterDeactivateEvent?.Invoke( argument );
        }

        // OnActivate
        protected virtual void OnBeforeActivate(object? argument) {
        }
        protected abstract void OnActivate(object? argument);
        protected virtual void OnAfterActivate(object? argument) {
        }
        protected virtual void OnBeforeDeactivate(object? argument) {
        }
        protected abstract void OnDeactivate(object? argument);
        protected virtual void OnAfterDeactivate(object? argument) {
        }

        // AddChild
        protected virtual void AddChild(TThis child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have no child {child} node" ).Valid( !Children.Contains( child ) );
            Children_.Add( child );
            Sort( Children_ );
            child.SetOwner( (TThis) this, argument );
        }
        protected virtual void RemoveChild(TThis child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have child {child} node" ).Valid( Children.Contains( child ) );
            child.RemoveOwner( (TThis) this, argument );
            Children_.Remove( child );
        }
        protected bool RemoveChild(Func<TThis, bool> predicate, object? argument = null) {
            var child = Children.LastOrDefault( predicate );
            if (child != null) {
                RemoveChild( child, argument );
                return true;
            }
            return false;
        }
        protected void RemoveChildren(IEnumerable<TThis> children, object? argument = null) {
            foreach (var child in children) {
                RemoveChild( child, argument );
            }
        }
        protected int RemoveChildren(Func<TThis, bool> predicate, object? argument = null) {
            var children = Children.Where( predicate ).Reverse().ToList();
            if (children.Any()) {
                RemoveChildren( children, argument );
                return children.Count;
            }
            return 0;
        }
        protected void RemoveSelf(object? argument = null) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            if (Parent != null) {
                Parent.RemoveChild( (TThis) this, argument );
            } else {
                Tree!.SetRoot( null, argument );
            }
        }

        // Sort
        protected virtual void Sort(List<TThis> children) {
            //children.Sort( (a, b) => Comparer<int>.Default.Compare( GetOrderOf( a ), GetOrderOf( b ) ) );
        }

    }
    public abstract class NodeBase2<TThis> : NodeBase<TThis> where TThis : NodeBase2<TThis> {

        // OnDescendantActivate
        public event Action<TThis, object?>? OnBeforeDescendantActivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantActivateEvent;
        public event Action<TThis, object?>? OnBeforeDescendantDeactivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantDeactivateEvent;

        // Constructor
        public NodeBase2() {
        }

        // Activate
        private protected sealed override void BeforeActivate(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantActivate( (TThis) this, argument );
            }
            base.BeforeActivate( argument );
        }
        private protected sealed override void AfterActivate(object? argument) {
            base.AfterActivate( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( (TThis) this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( (TThis) this, argument );
            }
        }
        private protected sealed override void BeforeDeactivate(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantDeactivate( (TThis) this, argument );
            }
            base.BeforeDeactivate( argument );
        }
        private protected sealed override void AfterDeactivate(object? argument) {
            base.AfterDeactivate( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDeactivate( (TThis) this, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
            }
        }

        // OnDescendantActivate
        protected abstract void OnBeforeDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnBeforeDescendantDeactivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantDeactivate(TThis descendant, object? argument);

    }
}

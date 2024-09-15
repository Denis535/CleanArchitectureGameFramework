#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public abstract class NodeBase {
        public enum State_ {
            Inactive,
            Activating,
            Active,
            Deactivating,
        }

        // Constructor
        internal NodeBase() {
        }

    }
    public abstract class NodeBase<T> : NodeBase where T : NodeBase<T> {

        // State
        public State_ State { get; private set; } = State_.Inactive;
        // Owner
        private object? Owner { get; set; }
        // Tree
        public ITree<T>? Tree => Owner as ITree<T>;
        // Parent
        public T? Parent => Owner as T;
        // Root
        [MemberNotNullWhen( false, nameof( Parent ) )] public bool IsRoot => Parent == null;
        public T Root => IsRoot ? (T) this : Parent.Root;
        // Ancestors
        public IEnumerable<T> Ancestors {
            get {
                if (Parent != null) {
                    yield return Parent;
                    foreach (var i in Parent.Ancestors) yield return i;
                }
            }
        }
        public IEnumerable<T> AncestorsAndSelf => Ancestors.Prepend( (T) this );
        // Children
        private List<T> Children_ { get; } = new List<T>( 0 );
        public IReadOnlyList<T> Children => Children_;
        // Descendants
        public IEnumerable<T> Descendants {
            get {
                foreach (var child in Children) {
                    yield return child;
                    foreach (var i in child.Descendants) yield return i;
                }
            }
        }
        public IEnumerable<T> DescendantsAndSelf => Descendants.Prepend( (T) this );
        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;
        // OnDescendantActivate
        public event Action<T, object?>? OnBeforeDescendantActivateEvent;
        public event Action<T, object?>? OnAfterDescendantActivateEvent;
        public event Action<T, object?>? OnBeforeDescendantDeactivateEvent;
        public event Action<T, object?>? OnAfterDescendantDeactivateEvent;

        // Constructor
        public NodeBase() {
        }

        // Activate
        internal void Activate(ITree<T> owner, object? argument) {
            Owner = owner;
            Activate( argument );
        }
        internal void Deactivate(ITree<T> owner, object? argument) {
            Assert.Argument.Message( $"Argument 'owner' ({owner}) must be valid" ).Valid( owner == Owner );
            Deactivate( argument );
            Owner = null;
        }

        // Activate
        internal void Activate(T owner, object? argument) {
            if (owner.State is State_.Active) {
                Owner = owner;
                Activate( argument );
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Owner = owner;
            }
        }
        internal void Deactivate(T owner, object? argument) {
            Assert.Argument.Message( $"Argument 'owner' ({owner}) must be valid" ).Valid( owner == Owner );
            if (owner.State is State_.Active) {
                Deactivate( argument );
                Owner = null;
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Owner = null;
            }
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( State is State_.Inactive );
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( (T) this, argument );
                ancestor.OnBeforeDescendantActivate( (T) this, argument );
            }
            OnBeforeActivateEvent?.Invoke( argument );
            OnBeforeActivate( argument );
            {
                State = State_.Activating;
                OnActivate( argument );
                foreach (var child in Children) {
                    child.Activate( argument );
                }
                State = State_.Active;
            }
            OnAfterActivate( argument );
            OnAfterActivateEvent?.Invoke( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( (T) this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( (T) this, argument );
            }
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( State is State_.Active );
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( (T) this, argument );
                ancestor.OnBeforeDescendantDeactivate( (T) this, argument );
            }
            OnBeforeDeactivateEvent?.Invoke( argument );
            OnBeforeDeactivate( argument );
            {
                State = State_.Deactivating;
                foreach (var child in Children.Reverse()) {
                    child.Deactivate( argument );
                }
                OnDeactivate( argument );
                State = State_.Inactive;
            }
            OnAfterDeactivate( argument );
            OnAfterDeactivateEvent?.Invoke( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDeactivate( (T) this, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( (T) this, argument );
            }
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

        // OnDescendantActivate
        protected abstract void OnBeforeDescendantActivate(T descendant, object? argument);
        protected abstract void OnAfterDescendantActivate(T descendant, object? argument);
        protected abstract void OnBeforeDescendantDeactivate(T descendant, object? argument);
        protected abstract void OnAfterDescendantDeactivate(T descendant, object? argument);

        // AddChild
        protected virtual void AddChild(T child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have no child {child} node" ).Valid( !Children.Contains( child ) );
            Children_.Add( child );
            Sort( Children_ );
            child.Activate( (T) this, argument );
        }
        protected virtual void RemoveChild(T child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have child {child} node" ).Valid( Children.Contains( child ) );
            child.Deactivate( (T) this, argument );
            Children_.Remove( child );
        }
        protected bool RemoveChild(Func<T, bool> predicate, object? argument = null) {
            var child = Children.LastOrDefault( predicate );
            if (child != null) {
                RemoveChild( child, argument );
                return true;
            }
            return false;
        }
        protected void RemoveChildren(IEnumerable<T> children, object? argument = null) {
            foreach (var child in children) {
                RemoveChild( child, argument );
            }
        }
        protected int RemoveChildren(Func<T, bool> predicate, object? argument = null) {
            var children = Children.Where( predicate ).Reverse().ToList();
            if (children.Any()) {
                RemoveChildren( children, argument );
                return children.Count;
            }
            return 0;
        }
        protected void RemoveSelf(object? argument = null) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            if (Owner is T parent) {
                parent.RemoveChild( (T) this, argument );
            } else {
                ((ITree<T>) Owner).SetRoot( null, argument );
            }
        }

        // Sort
        protected virtual void Sort(List<T> children) {
        }

    }
}

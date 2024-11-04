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

        // State
        public State_ State { get; private protected set; } = State_.Inactive;
        // Owner
        private protected object? Owner { get; set; }
        // Tree
        public ITree? Tree => Owner as ITree;
        // Root
        [MemberNotNullWhen( false, nameof( Parent ) )] public bool IsRoot => Parent == null;
        public NodeBase Root => Parent?.Root ?? this;
        // Parent
        public NodeBase? Parent => Owner as NodeBase;
        // Ancestors
        public IEnumerable<NodeBase> Ancestors {
            get {
                if (Parent != null) {
                    yield return Parent;
                    foreach (var i in Parent.Ancestors) yield return i;
                }
            }
        }
        public IEnumerable<NodeBase> AncestorsAndSelf => Ancestors.Prepend( this );
        // Children
        private protected List<NodeBase> Children_ { get; } = new List<NodeBase>( 0 );
        public IReadOnlyList<NodeBase> Children => Children_;
        // Descendants
        public IEnumerable<NodeBase> Descendants {
            get {
                foreach (var child in Children) {
                    yield return child;
                    foreach (var i in child.Descendants) yield return i;
                }
            }
        }
        public IEnumerable<NodeBase> DescendantsAndSelf => Descendants.Prepend( this );

        // Constructor
        internal NodeBase() {
        }

    }
    public abstract class NodeBase<TThis> : NodeBase where TThis : NodeBase<TThis> {

        // Tree
        public new ITree<TThis>? Tree => (ITree<TThis>?) base.Tree;
        // Root
        [MemberNotNullWhen( false, nameof( Parent ) )] public new bool IsRoot => base.IsRoot;
        public new TThis Root => (TThis) base.Root;
        // Parent
        public new TThis? Parent => (TThis?) base.Parent;
        // Ancestors
        public new IEnumerable<TThis> Ancestors => base.Ancestors.Cast<TThis>();
        public new IEnumerable<TThis> AncestorsAndSelf => base.AncestorsAndSelf.Cast<TThis>();
        // Children
        public new IEnumerable<TThis> Children => base.Children.Cast<TThis>();
        // Descendants
        public new IEnumerable<TThis> Descendants => base.Descendants.Cast<TThis>();
        public new IEnumerable<TThis> DescendantsAndSelf => base.DescendantsAndSelf.Cast<TThis>();
        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;
        // OnDescendantActivate
        public event Action<TThis, object?>? OnBeforeDescendantActivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantActivateEvent;
        public event Action<TThis, object?>? OnBeforeDescendantDeactivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantDeactivateEvent;

        // Constructor
        public NodeBase() {
        }
        protected virtual void DisposeWhenDeactivate() {
            (this as IDisposable)?.Dispose();
        }

        // Activate
        internal void Activate(ITree<TThis> owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( State is State_.Inactive );
            Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
            Owner = owner;
            Activate( argument );
        }
        internal void Deactivate(ITree<TThis> owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( State is State_.Active );
            Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
            Deactivate( argument );
            Owner = null;
        }

        // Activate
        internal void Activate(TThis owner, object? argument) {
            if (owner.State is State_.Active) {
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( State is State_.Inactive );
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Owner = owner;
                Activate( argument );
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( State is State_.Inactive );
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Owner = owner;
            }
        }
        internal void Deactivate(TThis owner, object? argument) {
            if (owner.State is State_.Active) {
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( State is State_.Active );
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Deactivate( argument );
                Owner = null;
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( State is State_.Inactive );
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Owner = null;
            }
        }

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( State is State_.Inactive );
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantActivate( (TThis) this, argument );
            }
            {
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
            }
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( (TThis) this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( (TThis) this, argument );
            }
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( State is State_.Active );
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantDeactivate( (TThis) this, argument );
            }
            {
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
            }
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDeactivate( (TThis) this, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
            }
            DisposeWhenDeactivate();
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
        protected abstract void OnBeforeDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnBeforeDescendantDeactivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantDeactivate(TThis descendant, object? argument);

        // AddChild
        protected virtual void AddChild(TThis child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have no child {child} node" ).Valid( !Children.Contains( child ) );
            Children_.Add( child );
            Sort( Children_ );
            child.Activate( (TThis) this, argument );
        }
        protected virtual void RemoveChild(TThis child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have child {child} node" ).Valid( Children.Contains( child ) );
            child.Deactivate( (TThis) this, argument );
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
            if (Owner is TThis parent) {
                parent.RemoveChild( (TThis) this, argument );
            } else {
                ((ITree<TThis>) Owner).SetRoot( null, argument );
            }
        }

        // Sort
        protected virtual void Sort(List<NodeBase> children) {
            //children.Sort( (a, b) => Comparer<int>.Default.Compare( GetOrderOf( a ), GetOrderOf( b ) ) );
        }

    }
}

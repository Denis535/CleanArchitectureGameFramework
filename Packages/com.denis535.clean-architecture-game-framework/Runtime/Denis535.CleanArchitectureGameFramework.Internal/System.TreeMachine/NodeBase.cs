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

        // Owner
        private object? Owner { get; set; } = null;
        // Activity
        public Activity_ Activity { get; private set; } = Activity_.Inactive;

        // Tree
        public ITree<TThis>? Tree => (ITree<TThis>?) Root?.Owner;

        // Root
        [MemberNotNullWhen( false, nameof( Parent ) )] public bool IsRoot => Parent == null;
        public TThis Root => Parent?.Root ?? (TThis) this;

        // Parent
        public TThis? Parent => Owner as TThis;
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

        // Attach
        internal void Attach(ITree<TThis> owner, object? argument) {
            if (true) {
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                Owner = owner;
                OnAttach( argument );
                Activate( argument );
            }
        }
        internal void Detach(ITree<TThis> owner, object? argument) {
            if (true) {
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
                Deactivate( argument );
                OnDetach( argument );
                Owner = null;
            }
        }

        // Attach
        private void Attach(TThis owner, object? argument) {
            if (owner.Activity is Activity_.Active) {
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                Owner = owner;
                OnAttach( argument );
                Activate( argument );
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                Owner = owner;
                OnAttach( argument );
            }
        }
        private void Detach(TThis owner, object? argument) {
            if (owner.Activity is Activity_.Active) {
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
                Deactivate( argument );
                OnDetach( argument );
                Owner = null;
            } else {
                Assert.Argument.Message( $"Argument 'argument' ({argument}) must be null" ).Valid( argument == null );
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                OnDetach( argument );
                Owner = null;
            }
        }

        // OnAttach
        protected abstract void OnAttach(object? argument);
        protected abstract void OnDetach(object? argument);

        // Activate
        private void Activate(object? argument) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"Node {this} must have owner with valid activity" ).Valid( (Owner is ITree<TThis>) || ((NodeBase<TThis>?) Owner)!.Activity is Activity_.Active or Activity_.Activating );
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            OnBeforeActivateInternal( argument );
            Activity = Activity_.Activating;
            {
                OnActivate( argument );
                foreach (var child in Children) {
                    child.Activate( argument );
                }
            }
            Activity = Activity_.Active;
            OnAfterActivateInternal( argument );
        }
        private void Deactivate(object? argument) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"Node {this} must have owner with valid activity" ).Valid( (Owner is ITree<TThis>) || ((NodeBase<TThis>?) Owner)!.Activity is Activity_.Active or Activity_.Deactivating );
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
            OnBeforeDeactivateInternal( argument );
            Activity = Activity_.Deactivating;
            {
                foreach (var child in Children.Reverse()) {
                    child.Deactivate( argument );
                }
                OnDeactivate( argument );
            }
            Activity = Activity_.Inactive;
            OnAfterDeactivateInternal( argument );
        }

        // OnActivate
        private protected virtual void OnBeforeActivateInternal(object? argument) {
            OnBeforeActivateEvent?.Invoke( argument );
            OnBeforeActivate( argument );
        }
        private protected virtual void OnAfterActivateInternal(object? argument) {
            OnAfterActivate( argument );
            OnAfterActivateEvent?.Invoke( argument );
        }
        private protected virtual void OnBeforeDeactivateInternal(object? argument) {
            OnBeforeDeactivateEvent?.Invoke( argument );
            OnBeforeDeactivate( argument );
        }
        private protected virtual void OnAfterDeactivateInternal(object? argument) {
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
            child.Attach( (TThis) this, argument );
        }
        protected virtual void RemoveChild(TThis child, object? argument = null) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have child {child} node" ).Valid( Children.Contains( child ) );
            child.Detach( (TThis) this, argument );
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
}

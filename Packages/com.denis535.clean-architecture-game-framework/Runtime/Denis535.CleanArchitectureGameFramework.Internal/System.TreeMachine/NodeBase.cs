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
        private protected object? Owner { get; private set; } = null;
        // Activity
        public Activity_ Activity { get; private protected set; } = Activity_.Inactive;

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

        // OnAttach
        public event Action<object?>? OnBeforeAttachEvent;
        public event Action<object?>? OnAfterAttachEvent;
        public event Action<object?>? OnBeforeDetachEvent;
        public event Action<object?>? OnAfterDetachEvent;

        // Constructor
        private protected NodeBase() {
        }

        // Attach
        internal void Attach(ITree<TThis> owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            {
                Owner = owner;
                OnBeforeAttach( argument );
                OnAttach( argument );
                OnAfterAttach( argument );
            }
            Activate( argument );
        }
        internal void Detach(ITree<TThis> owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
            Deactivate( argument );
            {
                OnBeforeDetach( argument );
                OnDetach( argument );
                OnAfterDetach( argument );
                Owner = null;
            }
        }

        // Attach
        private void Attach(TThis owner, object? argument) {
            Assert.Operation.Message( $"Node {this} must have no owner" ).Valid( Owner == null );
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            if (owner.Activity is Activity_.Active) {
                {
                    Owner = owner;
                    OnBeforeAttach( argument );
                    OnAttach( argument );
                    OnAfterAttach( argument );
                }
                Activate( argument );
            } else {
                {
                    Owner = owner;
                    OnBeforeAttach( argument );
                    OnAttach( argument );
                    OnAfterAttach( argument );
                }
            }
        }
        private void Detach(TThis owner, object? argument) {
            if (owner.Activity is Activity_.Active) {
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
                Deactivate( argument );
                {
                    OnBeforeDetach( argument );
                    OnDetach( argument );
                    OnAfterDetach( argument );
                    Owner = null;
                }
            } else {
                Assert.Operation.Message( $"Node {this} must have {owner} owner" ).Valid( Owner == owner );
                Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
                {
                    OnBeforeDetach( argument );
                    OnDetach( argument );
                    OnAfterDetach( argument );
                    Owner = null;
                }
            }
        }

        // OnAttach
        protected virtual void OnBeforeAttach(object? argument) {
            OnBeforeAttachEvent?.Invoke( argument );
        }
        protected abstract void OnAttach(object? argument);
        protected virtual void OnAfterAttach(object? argument) {
            OnAfterAttachEvent?.Invoke( argument );
        }
        protected virtual void OnBeforeDetach(object? argument) {
            OnBeforeDetachEvent?.Invoke( argument );
        }
        protected abstract void OnDetach(object? argument);
        protected virtual void OnAfterDetach(object? argument) {
            OnAfterDetachEvent?.Invoke( argument );
        }

        // Activate
        private protected abstract void Activate(object? argument);
        private protected abstract void Deactivate(object? argument);

        // AddChild
        protected virtual void AddChild(TThis child, object? argument) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have no child {child} node" ).Valid( !Children.Contains( child ) );
            Children_.Add( child );
            Sort( Children_ );
            child.Attach( (TThis) this, argument );
        }
        protected virtual void RemoveChild(TThis child, object? argument) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Operation.Message( $"Node {this} must have child {child} node" ).Valid( Children.Contains( child ) );
            child.Detach( (TThis) this, argument );
            Children_.Remove( child );
        }
        protected bool RemoveChild(Func<TThis, bool> predicate, object? argument) {
            var child = Children.LastOrDefault( predicate );
            if (child != null) {
                RemoveChild( child, argument );
                return true;
            }
            return false;
        }
        protected int RemoveChildren(Func<TThis, bool> predicate, object? argument) {
            var children = Children.Where( predicate ).Reverse().ToList();
            foreach (var child in children) {
                RemoveChild( child, argument );
            }
            return children.Count;
        }
        protected void RemoveSelf(object? argument) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            if (Parent != null) {
                Parent.RemoveChild( (TThis) this, argument );
            } else {
                Tree!.RemoveRoot( (TThis) this, argument );
            }
        }

        // Sort
        protected virtual void Sort(List<TThis> children) {
            //children.Sort( (a, b) => Comparer<int>.Default.Compare( GetOrderOf( a ), GetOrderOf( b ) ) );
        }

    }
}

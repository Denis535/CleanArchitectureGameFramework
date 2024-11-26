#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITree<T> where T : NodeBase<T> {

        // Root
        protected T? Root { get; set; }

        // SetRoot
        protected internal void SetRoot(T? root, object? argument = null) {
            SetRoot( this, root, argument );
        }
        protected void AddRoot(T root, object? argument = null) {
            AddRoot( this, root, argument );
        }
        protected void RemoveRoot(T root, object? argument = null) {
            RemoveRoot( this, root, argument );
        }

        // Helpers
        protected static void SetRoot(ITree<T> tree, T? root, object? argument) {
            if (root != null) {
                Assert.Argument.Message( $"Argument 'root' ({root}) must be inactive" ).Valid( root.Activity is NodeBase<T>.Activity_.Inactive );
                if (tree.Root != null) {
                    tree.RemoveRoot( tree.Root, argument );
                }
                tree.AddRoot( root, argument );
            } else {
                if (tree.Root != null) {
                    tree.RemoveRoot( tree.Root, argument );
                }
            }
        }
        protected static void AddRoot(ITree<T> tree, T root, object? argument) {
            Assert.Argument.Message( $"Argument 'root' must be non-null" ).NotNull( root != null );
            Assert.Argument.Message( $"Argument 'root' must be inactive" ).Valid( root.Activity is NodeBase<T>.Activity_.Inactive );
            Assert.Operation.Message( $"Tree {tree} must have no root node" ).Valid( tree.Root == null );
            tree.Root = root;
            tree.Root.Attach( tree, argument );
        }
        protected static void RemoveRoot(ITree<T> tree, T root, object? argument) {
            Assert.Argument.Message( $"Argument 'root' must be non-null" ).NotNull( root != null );
            Assert.Argument.Message( $"Argument 'root' must be active" ).Valid( root.Activity is NodeBase<T>.Activity_.Active );
            Assert.Operation.Message( $"Tree {tree} must have root {root} node" ).Valid( tree.Root == root );
            tree.Root.Detach( tree, argument );
            tree.Root = null;
        }

    }
}

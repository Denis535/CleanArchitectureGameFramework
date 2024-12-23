#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITree<T> where T : NodeBase<T> {

        // Root
        protected T? Root { get; set; }

        // SetRoot
        protected void SetRoot(T? root, object? argument, Action<T>? callback);
        protected void AddRoot(T root, object? argument);
        protected internal void RemoveRoot(T root, object? argument, Action<T>? callback);

        // Helpers
        protected static void SetRoot(ITree<T> tree, T? root, object? argument, Action<T>? callback) {
            if (tree.Root != null) {
                tree.RemoveRoot( tree.Root, argument, callback );
            }
            if (root != null) {
                tree.AddRoot( root, argument );
            }
        }
        protected static void AddRoot(ITree<T> tree, T root, object? argument) {
            Assert.Argument.Message( $"Argument 'root' must be non-null" ).NotNull( root != null );
            Assert.Operation.Message( $"Tree {tree} must have no root node" ).Valid( tree.Root == null );
            tree.Root = root;
            tree.Root.Attach( tree, argument );
        }
        protected static void RemoveRoot(ITree<T> tree, T root, object? argument, Action<T>? callback) {
            Assert.Argument.Message( $"Argument 'root' must be non-null" ).NotNull( root != null );
            Assert.Operation.Message( $"Tree {tree} must have root {root} node" ).Valid( tree.Root == root );
            tree.Root.Detach( tree, argument );
            tree.Root = null;
            callback?.Invoke( root );
        }

    }
}

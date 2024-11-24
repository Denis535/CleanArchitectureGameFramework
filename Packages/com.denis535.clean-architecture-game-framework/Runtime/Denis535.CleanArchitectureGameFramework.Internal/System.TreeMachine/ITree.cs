#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITree<T> where T : NodeBase<T> {

        // Root
        protected T? Root { get; set; }

        // SetRoot
        protected internal void SetRoot(T? root, object? argument = null);

        // Helpers
        protected static void SetRoot(ITree<T> tree, T? root, object? argument) {
            if (root != null) {
                Assert.Argument.Message( $"Argument 'root' ({root}) must be inactive" ).Valid( root.Activity is NodeBase<T>.Activity_.Inactive );
                if (tree.Root != null) {
                    RemoveRootInternal( tree, tree.Root, argument );
                }
                SetRootInternal( tree, root, argument );
            } else {
                if (tree.Root != null) {
                    RemoveRootInternal( tree, tree.Root, argument );
                }
            }
        }
        protected static void SetRootInternal(ITree<T> tree, T root, object? argument) {
            Assert.Argument.Message( $"Argument 'root' must be non-null" ).NotNull( root != null );
            Assert.Argument.Message( $"Argument 'root' must be active" ).Valid( root.Activity is NodeBase<T>.Activity_.Inactive );
            Assert.Operation.Message( $"Tree {tree} must have no root node" ).Valid( tree.Root == null );
            tree.Root = root;
            tree.Root.Attach( tree, argument );
        }
        protected static void RemoveRootInternal(ITree<T> tree, T root, object? argument) {
            Assert.Argument.Message( $"Argument 'root' must be non-null" ).NotNull( root != null );
            Assert.Argument.Message( $"Argument 'root' must be active" ).Valid( root.Activity is NodeBase<T>.Activity_.Active );
            Assert.Operation.Message( $"Tree {tree} must have root {root} node" ).Valid( tree.Root == root );
            tree.Root.Detach( tree, argument );
            tree.Root = null;
        }

    }
}

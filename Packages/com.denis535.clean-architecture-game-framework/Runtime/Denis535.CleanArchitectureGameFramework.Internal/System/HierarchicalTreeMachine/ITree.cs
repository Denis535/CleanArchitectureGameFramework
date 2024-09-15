#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITree {
    }
    public interface ITree<T> : ITree where T : NodeBase<T> {

        // Root
        protected T? Root { get; set; }

        // SetRoot
        protected internal void SetRoot(T? root, object? argument = null);

        // Helpers
        protected static void SetRoot(ITree<T> tree, T? root, object? argument) {
            if (root != null) {
                Assert.Operation.Message( $"Tree {tree} must have no root" ).Valid( tree.Root == null );
                tree.Root = root;
                tree.Root.Activate( tree, argument );
            } else {
                Assert.Operation.Message( $"Tree {tree} must have root" ).Valid( tree.Root != null );
                tree.Root.Deactivate( tree, argument );
                tree.Root = null;
            }
        }

    }
}

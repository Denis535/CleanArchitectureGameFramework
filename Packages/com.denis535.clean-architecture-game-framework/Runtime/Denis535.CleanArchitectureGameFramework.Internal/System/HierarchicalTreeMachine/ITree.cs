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
            if (tree.Root != null) {
                tree.Root.Deactivate( tree, argument );
                tree.Root = null;
            }
            if (root != null) {
                tree.Root = root;
                tree.Root.Activate( tree, argument );
            }
        }

    }
}

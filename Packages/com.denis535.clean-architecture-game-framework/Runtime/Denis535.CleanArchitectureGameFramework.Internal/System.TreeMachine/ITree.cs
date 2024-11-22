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
            if (tree.Root != null) {
                tree.Root.RemoveOwner( tree, argument );
                tree.Root = null;
            }
            if (root != null) {
                tree.Root = root;
                tree.Root.SetOwner( tree, argument );
            }
        }

    }
}

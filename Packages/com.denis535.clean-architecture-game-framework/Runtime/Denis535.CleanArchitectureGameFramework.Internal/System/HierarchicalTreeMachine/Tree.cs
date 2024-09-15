#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Tree<T> : ITree<T> where T : NodeBase<T> {

        // Root
        T? ITree<T>.Root { get => Root; set => Root = value; }
        // Root
        public T? Root { get; private set; }

        // Constructor
        public Tree() {
        }

        // SetRoot
        void ITree<T>.SetRoot(T? root, object? argument) => SetRoot( root, argument );
        // SetRoot
        public virtual void SetRoot(T? root, object? argument = null) {
            ITree<T>.SetRoot( this, root, argument );
        }

    }
}

#nullable enable
namespace System {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class TreeBase<T> : ITree<T> where T : NodeBase<T> {

        // Root
        T? ITree<T>.Root { get => Root; set => Root = value; }
        // Root
        protected T? Root { get; private set; }

        // Constructor
        public TreeBase() {
        }

        // SetRoot
        void ITree<T>.SetRoot(T? root, object? argument) => SetRoot( root, argument );
        // SetRoot
        protected virtual void SetRoot(T? root, object? argument = null) {
            ITree<T>.SetRoot( this, root, argument );
        }

    }
}

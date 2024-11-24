#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public abstract class NodeBase2<TThis> : NodeBase<TThis> where TThis : NodeBase2<TThis> {

        // OnDescendantActivate
        public event Action<TThis, object?>? OnBeforeDescendantActivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantActivateEvent;
        public event Action<TThis, object?>? OnBeforeDescendantDeactivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantDeactivateEvent;

        // Constructor
        public NodeBase2() {
        }

        // OnActivate
        private protected sealed override void OnBeforeActivateInternal(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantActivate( (TThis) this, argument );
            }
            base.OnBeforeActivateInternal( argument );
        }
        private protected sealed override void OnAfterActivateInternal(object? argument) {
            base.OnAfterActivateInternal( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( (TThis) this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( (TThis) this, argument );
            }
        }
        private protected sealed override void OnBeforeDeactivateInternal(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantDeactivate( (TThis) this, argument );
            }
            base.OnBeforeDeactivateInternal( argument );
        }
        private protected sealed override void OnAfterDeactivateInternal(object? argument) {
            base.OnAfterDeactivateInternal( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDeactivate( (TThis) this, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
            }
        }

        // OnDescendantActivate
        protected abstract void OnBeforeDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnBeforeDescendantDeactivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantDeactivate(TThis descendant, object? argument);

    }
}

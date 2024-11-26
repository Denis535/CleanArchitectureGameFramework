#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public abstract class NodeBase3<TThis> : NodeBase2<TThis> where TThis : NodeBase3<TThis> {

        // OnDescendantAttach
        public event Action<TThis, object?>? OnBeforeDescendantAttachEvent;
        public event Action<TThis, object?>? OnAfterDescendantAttachEvent;
        public event Action<TThis, object?>? OnBeforeDescendantDetachEvent;
        public event Action<TThis, object?>? OnAfterDescendantDetachEvent;

        // OnDescendantActivate
        public event Action<TThis, object?>? OnBeforeDescendantActivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantActivateEvent;
        public event Action<TThis, object?>? OnBeforeDescendantDeactivateEvent;
        public event Action<TThis, object?>? OnAfterDescendantDeactivateEvent;

        // Constructor
        public NodeBase3() {
        }

        // OnAttach
        protected override void OnBeforeAttach(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantAttachEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantAttach( (TThis) this, argument );
            }
            base.OnBeforeAttach( argument );
        }
        protected override void OnAfterAttach(object? argument) {
            base.OnAfterAttach( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantAttach( (TThis) this, argument );
                ancestor.OnAfterDescendantAttachEvent?.Invoke( (TThis) this, argument );
            }
        }
        protected override void OnBeforeDetach(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDetachEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantDetach( (TThis) this, argument );
            }
            base.OnBeforeDetach( argument );
        }
        protected override void OnAfterDetach(object? argument) {
            base.OnAfterDetach( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDetach( (TThis) this, argument );
                ancestor.OnAfterDescendantDetachEvent?.Invoke( (TThis) this, argument );
            }
        }

        // OnActivate
        protected override void OnBeforeActivate(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantActivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantActivate( (TThis) this, argument );
            }
            base.OnBeforeActivate( argument );
        }
        protected override void OnAfterActivate(object? argument) {
            base.OnAfterActivate( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantActivate( (TThis) this, argument );
                ancestor.OnAfterDescendantActivateEvent?.Invoke( (TThis) this, argument );
            }
        }
        protected override void OnBeforeDeactivate(object? argument) {
            foreach (var ancestor in Ancestors.Reverse()) {
                ancestor.OnBeforeDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
                ancestor.OnBeforeDescendantDeactivate( (TThis) this, argument );
            }
            base.OnBeforeDeactivate( argument );
        }
        protected override void OnAfterDeactivate(object? argument) {
            base.OnAfterDeactivate( argument );
            foreach (var ancestor in Ancestors) {
                ancestor.OnAfterDescendantDeactivate( (TThis) this, argument );
                ancestor.OnAfterDescendantDeactivateEvent?.Invoke( (TThis) this, argument );
            }
        }

        // OnDescendantAttach
        protected abstract void OnBeforeDescendantAttach(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantAttach(TThis descendant, object? argument);
        protected abstract void OnBeforeDescendantDetach(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantDetach(TThis descendant, object? argument);

        // OnDescendantActivate
        protected abstract void OnBeforeDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantActivate(TThis descendant, object? argument);
        protected abstract void OnBeforeDescendantDeactivate(TThis descendant, object? argument);
        protected abstract void OnAfterDescendantDeactivate(TThis descendant, object? argument);

    }
}

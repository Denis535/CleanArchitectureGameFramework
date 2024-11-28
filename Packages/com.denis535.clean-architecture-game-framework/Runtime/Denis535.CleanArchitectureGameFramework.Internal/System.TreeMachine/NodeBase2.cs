#nullable enable
namespace System.TreeMachine {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    public abstract class NodeBase2<TThis> : NodeBase<TThis> where TThis : NodeBase2<TThis> {

        // OnActivate
        public event Action<object?>? OnBeforeActivateEvent;
        public event Action<object?>? OnAfterActivateEvent;
        public event Action<object?>? OnBeforeDeactivateEvent;
        public event Action<object?>? OnAfterDeactivateEvent;

        // Constructor
        public NodeBase2() {
        }

        // Activate
        private protected sealed override void Activate(object? argument) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"Node {this} must have owner with valid activity" ).Valid( (Owner is ITree<TThis>) || ((NodeBase<TThis>) Owner).Activity is Activity_.Active or Activity_.Activating );
            Assert.Operation.Message( $"Node {this} must be inactive" ).Valid( Activity is Activity_.Inactive );
            OnBeforeActivate( argument );
            Activity = Activity_.Activating;
            {
                OnActivate( argument );
                foreach (var child in Children) {
                    child.Activate( argument );
                }
            }
            Activity = Activity_.Active;
            OnAfterActivate( argument );
        }
        private protected sealed override void Deactivate(object? argument) {
            Assert.Operation.Message( $"Node {this} must have owner" ).Valid( Owner != null );
            Assert.Operation.Message( $"Node {this} must have owner with valid activity" ).Valid( (Owner is ITree<TThis>) || ((NodeBase<TThis>) Owner).Activity is Activity_.Active or Activity_.Deactivating );
            Assert.Operation.Message( $"Node {this} must be active" ).Valid( Activity is Activity_.Active );
            OnBeforeDeactivate( argument );
            Activity = Activity_.Deactivating;
            {
                foreach (var child in Children.Reverse()) {
                    child.Deactivate( argument );
                }
                OnDeactivate( argument );
            }
            Activity = Activity_.Inactive;
            OnAfterDeactivate( argument );
        }

        // OnActivate
        protected virtual void OnBeforeActivate(object? argument) {
            OnBeforeActivateEvent?.Invoke( argument );
        }
        protected abstract void OnActivate(object? argument);
        protected virtual void OnAfterActivate(object? argument) {
            OnAfterActivateEvent?.Invoke( argument );
        }
        protected virtual void OnBeforeDeactivate(object? argument) {
            OnBeforeDeactivateEvent?.Invoke( argument );
        }
        protected abstract void OnDeactivate(object? argument);
        protected virtual void OnAfterDeactivate(object? argument) {
            OnAfterDeactivateEvent?.Invoke( argument );
        }

    }
}

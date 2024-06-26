#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIViewBase : Disposable {

        // IsActive
        internal abstract bool IsActive { get; }
        // IsShown
        internal abstract bool IsShown { get; }
        // Parent
        internal abstract UIViewBase? Parent { get; }
        // Children
        internal abstract IEnumerable<UIViewBase> Children { get; }

        // Constructor
        public UIViewBase() {
        }
        public override void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be inactive" ).Valid( !IsActive );
            foreach (var child in Children) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            base.Dispose();
        }

    }
}

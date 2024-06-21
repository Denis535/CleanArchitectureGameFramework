#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UIViewBase : Disposable {

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
            base.Dispose();
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.UIElements;

    public interface IUIView : IDisposable {

        // System
        public bool IsDisposed { get; }
        // IsAttached
        public bool IsAttached => ((VisualElement) this).panel != null;
        // IsShown
        public bool IsShown => ((VisualElement) this).parent != null;

    }
    public abstract class UIViewBase : VisualElement, IUIView {

        private CancellationTokenSource? disposeCancellationTokenSource;

        // System
        public bool IsDisposed { get; private set; }
        public CancellationToken DisposeCancellationToken {
            get {
                if (disposeCancellationTokenSource == null) {
                    disposeCancellationTokenSource = new CancellationTokenSource();
                    if (IsDisposed) disposeCancellationTokenSource.Cancel();
                }
                return disposeCancellationTokenSource.Token;
            }
        }
        // IsAttached
        public bool IsAttached => panel != null;
        // IsShown
        public bool IsShown => parent != null;

        // Constructor
        public UIViewBase() {
        }
        public UIViewBase(string name, params string[] classes) {
            this.name = name;
            foreach (var @class in classes) {
                AddToClassList( @class );
            }
        }
        public virtual void Dispose() {
            Assert.Operation.Message( $"View {this} must be non-disposed" ).NotDisposed( !IsDisposed );
            Assert.Operation.Message( $"View {this} must be non-attached" ).Valid( !IsAttached );
            foreach (var child in this.GetChildren()) {
                Assert.Operation.Message( $"Child {child} must be disposed" ).Valid( child.IsDisposed );
            }
            IsDisposed = true;
            disposeCancellationTokenSource?.Cancel();
        }

    }
}

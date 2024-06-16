#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase2<TState> : UIScreenBase where TState : Enum {

        private TState state = default!;

        // System
        protected IDependencyContainer Container { get; }
        protected UIDocument Document { get; }
        protected AudioSource AudioSource { get; }
        // State
        public TState State {
            get => state;
            protected internal set {
                if (!EqualityComparer<TState>.Default.Equals( value, state )) {
                    state = value;
                    OnStateChange( state );
                    OnStateChangeEvent?.Invoke( state );
                }
            }
        }
        public event Action<TState>? OnStateChangeEvent;

        // Constructor
        public UIScreenBase2(IDependencyContainer container, UIDocument document, AudioSource audioSource) {
            Container = container;
            Document = document;
            AudioSource = audioSource;
        }
        public override void Dispose() {
            base.Dispose();
        }

        // AddWidget
        public override void AddWidget(UIWidgetBase widget, object? argument = null) {
            base.AddWidget( widget, argument );
            Document.rootVisualElement.Add( widget.View! );
        }
        public override void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            if (Document && Document.rootVisualElement != null) Document.rootVisualElement.Remove( widget.View! );
            base.RemoveWidget( widget, argument );
        }

        // OnStateChange
        protected abstract void OnStateChange(TState state);

    }
}

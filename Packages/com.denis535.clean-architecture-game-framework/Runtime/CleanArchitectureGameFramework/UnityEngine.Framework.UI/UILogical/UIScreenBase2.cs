#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase2 : UIScreenBase {

        // Container
        protected IDependencyContainer Container { get; }
        // Document
        protected UIDocument Document { get; }
        // AudioSource
        protected AudioSource AudioSource { get; }

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
            Document.Add( widget.View! );
        }
        public override void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            if (Document && Document.rootVisualElement != null) Document.Remove( widget.View! );
            base.RemoveWidget( widget, argument );
        }

    }
}

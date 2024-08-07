#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase2 : UIScreenBase {

        // System
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
        protected internal override void AddWidget(UIWidgetBase widget, object? argument = null) {
            base.AddWidget( widget, argument );
            Document.rootVisualElement.Add( widget.View! );
        }
        protected internal override void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            if (Document && Document.rootVisualElement != null) Document.rootVisualElement.Remove( widget.View! );
            base.RemoveWidget( widget, argument );
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIScreenBase2 : UIScreenBase {

        // Document
        protected UIDocument Document { get; }
        // AudioSource
        protected AudioSource AudioSource { get; }
        // Container
        protected abstract IDependencyContainer Container { get; }

        // Constructor
        public UIScreenBase2(UIDocument document, AudioSource audioSource) {
            Document = document;
            AudioSource = audioSource;
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

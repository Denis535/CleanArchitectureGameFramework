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
        protected UIDocument Document { get; }
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
            Document.rootVisualElement.Add( (UIViewBase2) widget.View! );
        }
        public override void RemoveWidget(UIWidgetBase widget, object? argument = null) {
            if (Document && Document.rootVisualElement != null) Document.rootVisualElement.Remove( (UIViewBase2) widget.View! );
            base.RemoveWidget( widget, argument );
        }

    }
}

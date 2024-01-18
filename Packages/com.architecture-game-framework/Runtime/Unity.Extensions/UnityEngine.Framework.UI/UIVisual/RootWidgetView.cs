#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class RootWidgetView : UIViewBase {

        // VisualElement
        public override VisualElement VisualElement { get; }
        public ElementWrapper Widget { get; }
        public SlotWrapper Container { get; }
        public SlotWrapper ModalContainer { get; }

        // Constructor
        public RootWidgetView() {
            VisualElement = CreateVisualElement( out var widget, out var container, out var modalContainer );
            Widget = widget.Wrap();
            Container = container.AsSlot();
            ModalContainer = modalContainer.AsSlot();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers
        private static VisualElement CreateVisualElement(out VisualElement widget, out VisualElement container, out VisualElement modalContainer) {
            widget = new VisualElement();
            widget.name = "root-widget";
            widget.AddToClassList( "root-widget" );
            widget.pickingMode = PickingMode.Ignore;
            {
                container = new VisualElement();
                container.name = "container";
                container.AddToClassList( "container" );
                container.pickingMode = PickingMode.Ignore;
                widget.Add( container );
            }
            {
                modalContainer = new VisualElement();
                modalContainer.name = "modal-container";
                modalContainer.AddToClassList( "modal-container" );
                modalContainer.pickingMode = PickingMode.Ignore;
                widget.Add( modalContainer );
            }
            return widget;
        }

    }
}

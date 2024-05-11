#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class UIRootWidgetViewBase : UIViewBase {

        // Root
        public abstract ElementWrapper Root { get; }
        public abstract ViewListSlotWrapper<UIViewBase> ViewSlot { get; }
        public abstract ViewListSlotWrapper<UIViewBase> ModalViewSlot { get; }

        // Constructor
        public UIRootWidgetViewBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

    }
    public class UIRootWidgetView : UIRootWidgetViewBase {

        // Root
        public override ElementWrapper Root { get; }
        public override ViewListSlotWrapper<UIViewBase> ViewSlot { get; }
        public override ViewListSlotWrapper<UIViewBase> ModalViewSlot { get; }

        // Constructor
        public UIRootWidgetView() {
            VisualElement = CreateVisualElement( out var root, out var viewSlot, out var modalViewSlot );
            Root = root.Wrap();
            ViewSlot = viewSlot.AsViewListSlot<UIViewBase>();
            ModalViewSlot = modalViewSlot.AsViewListSlot<UIViewBase>();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // Helpers/CreateVisualElement
        protected static VisualElement CreateVisualElement(out VisualElement root, out VisualElement viewSlot, out VisualElement modalViewSlot) {
            root = new VisualElement();
            root.name = "root-widget";
            root.AddToClassList( "root-widget" );
            root.pickingMode = PickingMode.Ignore;
            {
                viewSlot = new VisualElement();
                viewSlot.name = "view-slot";
                viewSlot.AddToClassList( "view-slot" );
                viewSlot.pickingMode = PickingMode.Ignore;
                root.Add( viewSlot );
            }
            {
                modalViewSlot = new VisualElement();
                modalViewSlot.name = "modal-view-slot";
                modalViewSlot.AddToClassList( "modal-view-slot" );
                modalViewSlot.pickingMode = PickingMode.Ignore;
                root.Add( modalViewSlot );
            }
            return root;
        }

    }
}

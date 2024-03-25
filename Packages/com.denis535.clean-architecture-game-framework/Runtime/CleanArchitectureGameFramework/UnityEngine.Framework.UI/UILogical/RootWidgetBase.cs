#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class RootWidgetBase<TView> : UIWidgetBase<TView> where TView : RootWidgetViewBase {

        // Constructor
        public RootWidgetBase() {
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach(object? argument) {
        }
        public override void OnDetach(object? argument) {
        }

        // OnBeforeDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            base.OnAfterDescendantDetach( descendant );
        }

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child, object? argument) {
            base.__AttachChild__( child, argument );
        }
        protected internal override void __DetachChild__(UIWidgetBase child, object? argument) {
            base.__DetachChild__( child, argument );
        }

        // ShowDescendantWidget
        protected override void ShowDescendantWidget(UIWidgetBase widget) {
            base.ShowDescendantWidget( widget );
        }
        protected override void HideDescendantWidget(UIWidgetBase widget) {
            base.HideDescendantWidget( widget );
        }

    }
    public class RootWidget : RootWidgetBase<RootWidgetView> {

        // View
        protected internal override RootWidgetView View { get; }
        private List<UIWidgetBase> WidgetList_ { get; } = new List<UIWidgetBase>();
        private List<UIWidgetBase> ModalWidgetList_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> WidgetList => WidgetList_;
        public IReadOnlyList<UIWidgetBase> ModalWidgetList => ModalWidgetList_;

        // Constructor
        public RootWidget() {
            View = CreateView();
        }
        public override void Dispose() {
            base.Dispose();
        }

        // OnAttach
        public override void OnAttach(object? argument) {
            base.OnAttach( argument );
        }
        public override void OnDetach(object? argument) {
            base.OnDetach( argument );
        }

        // OnBeforeDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            base.OnAfterDescendantDetach( descendant );
        }

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child, object? argument) {
            base.__AttachChild__( child, argument );
        }
        protected internal override void __DetachChild__(UIWidgetBase child, object? argument) {
            base.__DetachChild__( child, argument );
        }

        // ShowDescendantWidget
        protected override void ShowDescendantWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    ModalWidgetList_.Add( widget );
                    View.ModalWidgetList.Add( widget.View );
                } else {
                    WidgetList_.Add( widget );
                    View.WidgetList.Add( widget.View );
                }
                {
                    var prev = (UIWidgetBase?) WidgetList.Concat( ModalWidgetList ).SkipLast( 1 ).LastOrDefault();
                    if (prev != null) prev.View!.VisualElement.SaveFocus();
                    RecalcVisibility();
                    var last = (UIWidgetBase?) WidgetList.Concat( ModalWidgetList ).LastOrDefault();
                    if (last != null) last.View!.VisualElement.LoadFocus();
                }
            }
        }
        protected override void HideDescendantWidget(UIWidgetBase widget) {
            if (widget.IsViewable) {
                if (widget.IsModal()) {
                    Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == ModalWidgetList.LastOrDefault() );
                    ModalWidgetList_.Remove( widget );
                    View.ModalWidgetList.Remove( widget.View );
                } else {
                    Assert.Operation.Message( $"Widget {widget} must be last" ).Valid( widget == WidgetList.LastOrDefault() );
                    WidgetList_.Remove( widget );
                    View.WidgetList.Remove( widget.View );
                }
                {
                    RecalcVisibility();
                    var last = (UIWidgetBase?) WidgetList.Concat( ModalWidgetList ).LastOrDefault();
                    if (last != null) last.View!.VisualElement.LoadFocus();
                }
            }
        }

        // RecalcVisibility
        protected virtual void RecalcVisibility() {
            foreach (var widget in WidgetList) {
                RecalcWidgetVisibility( widget, widget == WidgetList.Last() );
            }
            foreach (var widget in ModalWidgetList) {
                RecalcModalWidgetVisibility( widget, widget == ModalWidgetList.Last() );
            }
        }
        protected virtual void RecalcWidgetVisibility(UIWidgetBase widget, bool isLast) {
            if (!isLast) {
                // hide covered widgets
                widget.SetEnabled( true );
                widget.SetDisplayed( false );
            } else {
                // show new widget or unhide uncovered widget
                widget.SetEnabled( !ModalWidgetList.Any() );
                widget.SetDisplayed( true );
            }
        }
        protected virtual void RecalcModalWidgetVisibility(UIWidgetBase widget, bool isLast) {
            if (!isLast) {
                // hide covered widgets
                widget.SetEnabled( true );
                widget.SetDisplayed( false );
            } else {
                // show new widget or unhide uncovered widget
                widget.SetEnabled( true );
                widget.SetDisplayed( true );
            }
        }

        // Helpers
        protected virtual RootWidgetView CreateView() {
            var view = new RootWidgetView();
            view.Widget.OnEvent<NavigationSubmitEvent>( evt => {
                var button = (Button?) evt.target;
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            view.Widget.OnEvent<NavigationCancelEvent>( evt => {
                var widget = ((VisualElement) evt.target).GetAncestorsAndSelf().FirstOrDefault( IsWidget );
                var button = widget?.Query<Button>().Where( IsCancel ).First();
                if (button != null) {
                    Click( button );
                    evt.StopPropagation();
                }
            }, TrickleDown.TrickleDown );
            return view;
        }
        protected static bool IsWidget(VisualElement element) {
            if (element.enabledInHierarchy) {
                if (element.name != null) {
                    return element.name.Contains( "widget", StringComparison.CurrentCultureIgnoreCase );
                }
                return element.GetType().Name.Contains( "widget", StringComparison.CurrentCultureIgnoreCase );
            }
            return false;
        }
        protected static bool IsCancel(Button button) {
            if (button.enabledInHierarchy) {
                if (button.name != null) {
                    return button.name is "resume" or "cancel" or "no" or "back" or "exit" or "quit";
                }
                if (button.text != null) {
                    return button.text.Contains( "resume", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "cancel", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "no", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "back", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "exit", StringComparison.CurrentCultureIgnoreCase ) ||
                           button.text.Contains( "quit", StringComparison.CurrentCultureIgnoreCase );
                }
            }
            return false;
        }
        protected static void Click(Button button) {
            using (var click = ClickEvent.GetPooled()) {
                click.target = button;
                button.SendEvent( click );
            }
        }

    }
}

#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public static class UIScreenExtensions {

        // AttachWidget
        public static void AttachWidget(this UIScreenBase sceen, UIWidgetBase widget) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {sceen} must have no widget" ).Valid( sceen.Widget == null );
            sceen.__AttachWidget__( widget );
        }

        // DetachWidget
        public static void DetachWidget(this UIScreenBase sceen) {
            Assert.Object.Message( $"Screen {sceen} must have widget" ).Valid( sceen.Widget != null );
            sceen.__DetachWidget__( sceen.Widget );
        }
        public static void DetachWidget<T>(this UIScreenBase sceen) where T : UIWidgetBase {
            Assert.Object.Message( $"Screen {sceen} must have widget" ).Valid( sceen.Widget != null );
            Assert.Object.Message( $"Screen {sceen} must have {typeof( T )} widget" ).Valid( sceen.Widget is T );
            sceen.__DetachWidget__( sceen.Widget );
        }
        public static void DetachWidget(this UIScreenBase sceen, UIWidgetBase widget) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {sceen} must have widget" ).Valid( sceen.Widget != null );
            Assert.Object.Message( $"Screen {sceen} must have {widget} widget" ).Valid( sceen.Widget == widget );
            sceen.__DetachWidget__( widget );
        }

        // OnDescendantWidgetAttach
        public static void OnBeforeDescendantWidgetAttach(this UIScreenBase screen, Action<UIWidgetBase> callback) {
            screen.OnBeforeDescendantWidgetAttachEvent += callback;
        }
        public static void OnAfterDescendantWidgetAttach(this UIScreenBase screen, Action<UIWidgetBase> callback) {
            screen.OnAfterDescendantWidgetAttachEvent += callback;
        }
        public static void OnBeforeDescendantWidgetDetach(this UIScreenBase screen, Action<UIWidgetBase> callback) {
            screen.OnBeforeDescendantWidgetDetachEvent += callback;
        }
        public static void OnAfterDescendantWidgetDetach(this UIScreenBase screen, Action<UIWidgetBase> callback) {
            screen.OnAfterDescendantWidgetDetachEvent += callback;
        }

        // OnDescendantWidgetAttach
        public static void OnBeforeDescendantWidgetAttach<TWidget>(this UIScreenBase screen, Action<TWidget> callback) where TWidget : UIWidgetBase {
            screen.OnBeforeDescendantWidgetAttachEvent += descendant => {
                if (descendant is TWidget descendant_) callback( descendant_ );
            };
        }
        public static void OnAfterDescendantWidgetAttach<TWidget>(this UIScreenBase screen, Action<TWidget> callback) where TWidget : UIWidgetBase {
            screen.OnAfterDescendantWidgetAttachEvent += descendant => {
                if (descendant is TWidget descendant_) callback( descendant_ );
            };
        }
        public static void OnBeforeDescendantWidgetDetach<TWidget>(this UIScreenBase screen, Action<TWidget> callback) where TWidget : UIWidgetBase {
            screen.OnBeforeDescendantWidgetDetachEvent += descendant => {
                if (descendant is TWidget descendant_) callback( descendant_ );
            };
        }
        public static void OnAfterDescendantWidgetDetach<TWidget>(this UIScreenBase screen, Action<TWidget> callback) where TWidget : UIWidgetBase {
            screen.OnAfterDescendantWidgetDetachEvent += descendant => {
                if (descendant is TWidget descendant_) callback( descendant_ );
            };
        }

    }
}

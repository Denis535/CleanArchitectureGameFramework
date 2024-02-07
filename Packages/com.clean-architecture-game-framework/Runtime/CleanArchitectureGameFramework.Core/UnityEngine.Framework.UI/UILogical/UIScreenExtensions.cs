#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class UIScreenExtensions {

        // AttachWidget
        public static void AttachWidget(this UIScreenBase sceen, UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {sceen} must have no widget" ).Valid( sceen.Widget == null );
            sceen.__AttachWidget__( widget, argument );
        }

        // DetachWidget
        public static void DetachWidget(this UIScreenBase sceen, object? argument = null) {
            Assert.Object.Message( $"Screen {sceen} must have widget" ).Valid( sceen.Widget != null );
            sceen.__DetachWidget__( sceen.Widget, argument );
        }
        public static void DetachWidget<T>(this UIScreenBase sceen, object? argument = null) where T : UIWidgetBase {
            Assert.Object.Message( $"Screen {sceen} must have widget" ).Valid( sceen.Widget != null );
            Assert.Object.Message( $"Screen {sceen} must have {typeof( T )} widget" ).Valid( sceen.Widget is T );
            sceen.__DetachWidget__( sceen.Widget, argument );
        }
        public static void DetachWidget(this UIScreenBase sceen, UIWidgetBase widget, object? argument = null) {
            Assert.Argument.Message( $"Argument 'widget' must be non-null" ).NotNull( widget != null );
            Assert.Object.Message( $"Screen {sceen} must have widget" ).Valid( sceen.Widget != null );
            Assert.Object.Message( $"Screen {sceen} must have {widget} widget" ).Valid( sceen.Widget == widget );
            sceen.__DetachWidget__( widget, argument );
        }

    }
}

#nullable enable
namespace System.Text {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal static class StringBuilderExtensions {

        public static StringBuilder AppendIndent(this StringBuilder builder, int indent) {
            builder.Append( ' ', indent * 4 );
            return builder;
        }

    }
}

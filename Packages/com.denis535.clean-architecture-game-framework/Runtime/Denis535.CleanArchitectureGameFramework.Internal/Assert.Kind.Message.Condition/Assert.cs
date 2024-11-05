#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class Assert {
        public static class Argument {
            public static Assertions.Argument Message(FormattableString? value) => new Assertions.Argument( value );
        }
        public static class Operation {
            public static Assertions.Operation Message(FormattableString? value) => new Assertions.Operation( value );
        }
    }
}
